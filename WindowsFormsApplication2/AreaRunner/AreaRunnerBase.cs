using Process.NET.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication2.AreaRunner.LockedAction;
using WindowsFormsApplication2.AreaRunner.PlayerParallelActions;
using WindowsFormsApplication2.ImageProcessor;
using WindowsFormsApplication2.Logger;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.AreaRunner {

    public class History<TMapInfo> : List<HistoryElement<TMapInfo>> where TMapInfo : MoveInfoBase {
        int ID = 0;
        public new void Add(HistoryElement<TMapInfo> historyElement) {
            historyElement.ID = ID;
            ID++;
            Add(historyElement);
        }
    }

    public class HistoryElement<TMapInfo> where TMapInfo : MoveInfoBase {
        public int ID { get; set; } = 0;
        public IEnumerable<TMapInfo> MoveInfos { get; set; }
    }

    public abstract class AreaRunnerBase {

        protected abstract double MainAreaOrientation { get; }
        const int MAX_HISTORY_LENGTH = 40;
        const int CORRECTION_COOLDOWN = 3000;
        protected readonly ILogger logger;
        protected History<MapDirectionMoveInfo> fogHistory = new History<MapDirectionMoveInfo>();
        protected History<MapMoveInfo> mapHistory = new History<MapMoveInfo>();

        //protected ActionLockerFactory actionLockerFactory;
        readonly LockObserver lockObserver = new LockObserver();

        protected GameMapFogProcessor gameMapFogProcessor = new GameMapFogProcessor();
        protected GameMapProcessor mapProcessor = new GameMapProcessor();
        protected LootProcessor lootProcessor = new LootProcessor();

        protected AttackPusher AttackPusher = new AttackPusher();
        protected FlaskPusher FlaskPusher = new FlaskPusher();
        protected SkillPusher SkillPusher = new SkillPusher();

        public AreaRunnerBase(ILogger logger) {
            mapProcessor.OnResult += OnMapProcessorResult;
            gameMapFogProcessor.OnResult += OnFogResult;
            lootProcessor.OnResult += OnLootResult;
            lockObserver.Add(lootProcessor);
            //actionLockerFactory = new ActionLockerFactory(logger);
            this.logger = logger;
        }

        private void OnLootResult(object sender, ImageProcessorEventArgs<GameMapProcessorResult<LootMoveResult>> e) {
            var res = e.ImageProcessorResult.VectorizationResult;
            if (!res.Any())
                return;
            var point = res.OrderBy(a => a.Vector.Length()).FirstOrDefault().LootCord;
            AvailableInput.MouseMove(point);
            //AvailableInput.Input(Settings.MoveKey);
            AvailableInput.Input(InputCodes.LButton);

        }

        void AddToHistory<T>(List<HistoryElement<T>> history, HistoryElement<T> historyElement) where T : MoveInfoBase {
            if (history.Count > MAX_HISTORY_LENGTH) {
                history.Remove(history.Last());
            }
            else {
                historyElement.ID += 1;
                history.Add(historyElement);
            }
        }
        void AddToFogHistory(HistoryElement<MapDirectionMoveInfo> historyElement) {
            AddToHistory(fogHistory, historyElement);
        }
        void AddToMapHistory(HistoryElement<MapMoveInfo> historyElement) {
            AddToHistory(mapHistory, historyElement);
        }

        void OnMapProcessorResult(object sender, ImageProcessorEventArgs<GameMapProcessorResult<MapMoveInfo>> e) {
            var vectors = e.ImageProcessorResult.VectorizationResult;
            AddToMapHistory(new HistoryElement<MapMoveInfo>() { MoveInfos = vectors });
        }

        //double FindMostPerspectiveAngle(IEnumerable<MapMoveInfo> moveInfo) {
        //    var resultangle = moveInfo.OrderBy(a => a.Angle).FirstOrDefault().Angle; //exit
        //    foreach (var info in moveInfo) {
        //        var perspective = info.Angle;
        //        for (int i = mapHistory.Count - 1; i >= mapHistory.Count / 2; i--) {
        //            foreach (var angle in Extensions.AngleIterator(perspective.ToAngle(-90), perspective.ToAngle(90))) {
        //                var vector = mapHistory[i].MoveInfos.FirstOrDefault(a => a.Angle == angle);
        //                if (vector.IntersectionType == IntersectionType.NewAreaFlag) {
        //                    resultangle = vector.Angle;
        //                }
        //            }
        //        }
        //    }
        //    return resultangle;
        //}

        protected virtual void OnAreaCreated() {
            //var point = NativeApiWrapper.GetScreenRotatedPoint((int)MainAreaOrientation);
            ////tionLockerFactory.Wait(1000);
            //window.Mouse.MoveTo(point.X, point.Y);
            ////window.Mouse.ClickLeft();
            //window.Keyboard.Press(Process.NET.Native.Types.Keys.Q);
            //window.Mouse.MoveTo(point.X+100, point.Y);
        }

        protected virtual void OnAreaEnded() {

        }

        protected abstract MapDirectionMoveInfo ProcessNextMarker(IEnumerable<MapDirectionMoveInfo> moveInfo);

        protected abstract double MaxAngle { get; }

        protected abstract double MinAngle { get; }

        void OnFogResult(object sender, ImageProcessorEventArgs<GameMapProcessorResult<MapDirectionMoveInfo>> e) {
            var calcRes = e.ImageProcessorResult.VectorizationResult.ToList();
            var itemsToRemove = calcRes.Where(a => a.Angle >= MaxAngle || a.Angle<=MinAngle).ToList();
            foreach (var item in itemsToRemove) {
                calcRes.Remove(item);
            }
            AddToFogHistory(new HistoryElement<MapDirectionMoveInfo>() { MoveInfos = calcRes });
            if (!mapHistory.Any())
                return;
            if (mapHistory.Count == 1)
                OnAreaCreated();
            if (lockObserver.Locked)
                return;
            var mainAngle = MainAreaOrientation;
            IEnumerable<MapDirectionMoveInfo> vectorizationResult = null; ;
            if (!calcRes.Any()) {
                var lastSuccessResult = fogHistory.OrderByDescending(a => a.ID).Where(a => a.MoveInfos.Any());
                if (lastSuccessResult.Any()) {
                    vectorizationResult = lastSuccessResult.OrderBy(a => a.ID).Last().MoveInfos;
                }
            }
            else {
                vectorizationResult = calcRes;
            }
            if (vectorizationResult == null) {
                OnAreaEnded();
                return;
            }
            var nextMarker = ProcessNextMarker(vectorizationResult);
            mainAngle = nextMarker.CenterAngle;
            AttackPusher.CenterAngle = nextMarker.CenterAngle;


            //logger.WriteLog("perspective" + mainAngle.ToString());
            //var vector = mapHistory.Last().MoveInfos.FirstOrDefault(a => a.Angle == mainAngle.ToAngle(90));
            //var oppositeVector = mapHistory.Last().MoveInfos.FirstOrDefault(a => a.Angle == mainAngle.ToAngle(180 + 90));
            //var vectorDistance = vector.Vector.Length();
            //var oppositeVectorDistance = oppositeVector.Vector.Length();
            //if (vectorDistance + oppositeVectorDistance < 5) {
            //    mainAngle = mainAngle.ToAngle(180);
            //    logger.WriteLog("if wrong" + mainAngle.ToString());
            //}
            //else {
            //    var addSum = ((vectorDistance - oppositeVectorDistance) / (vectorDistance + oppositeVectorDistance)) * 90;
            //    mainAngle = mainAngle + addSum;
            //    logger.WriteLog("after correction" + mainAngle.ToString() + " at " + addSum.ToString());
            //}
            var point = NativeApiWrapper.GetScreenRotatedPoint((int)mainAngle);
            AvailableInput.MouseMove(point);
            AvailableInput.Input(Settings.MoveKey);
        }
    }
}
