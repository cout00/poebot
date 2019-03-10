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

        public event EventHandler AreaEnded;

        protected abstract double MainAreaOrientation { get; }
        const int MAX_HISTORY_LENGTH = 500;
        protected readonly ILogger logger;
        protected History<MapDirectionMoveInfo> fogHistory = new History<MapDirectionMoveInfo>();
        protected History<MapMoveInfo> mapHistory = new History<MapMoveInfo>();

        readonly LockObserver lockObserver = new LockObserver();

        protected GameMapFogProcessor gameMapFogProcessor = new GameMapFogProcessor();
        protected GameMapProcessor mapProcessor = new GameMapProcessor();
        protected LootProcessor2 lootProcessor = new LootProcessor2();

        protected AttackPusher AttackPusher = new AttackPusher();
        protected FlaskPusher FlaskPusher = new FlaskPusher();
        protected SkillPusher SkillPusher = new SkillPusher();

        int time = 0;

        public AreaRunnerBase(ILogger logger) : this() {
            this.logger = logger;
        }

        public AreaRunnerBase() {
            mapProcessor.OnResult += OnMapProcessorResult;
            gameMapFogProcessor.OnResult += OnFogResult;
            lootProcessor.OnResult += OnLootResult;
            lockObserver.Add(lootProcessor);
        }

        private void OnLootResult(object sender, ImageProcessorEventArgs<GameMapProcessorResult<LootMoveResult>> e) {
            var res = e.ImageProcessorResult.VectorizationResult;
            if (!res.Any())
                return;
            if (res.FirstOrDefault().IsLootEndPoint) {
                var point = res.OrderBy(a => a.Vector.Length()).FirstOrDefault().LootCord;
                AvailableInput.MouseMove(point);
                AvailableInput.Input(InputCodes.LButton);
            }
            else {
                var angle = res.OrderBy(a => a.Vector.Length()).FirstOrDefault().Angle;
                var point = NativeApiWrapper.GetScreenRotatedPoint((int)angle);
                AvailableInput.MouseMove(point);
                AvailableInput.Input(Settings.MoveKey);
            }
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


        protected virtual void OnAreaEnded() {
            Abort();
            AreaEnded?.Invoke(this, new EventArgs());
        }

        protected abstract MapDirectionMoveInfo ProcessNextMarker(IEnumerable<MapDirectionMoveInfo> moveInfo);

        protected abstract double MaxAngle { get; }

        protected abstract double MinAngle { get; }

        protected abstract int MaxAreaTime { get; }

        public void Abort() {
            mapProcessor.OnResult -= OnMapProcessorResult;
            gameMapFogProcessor.OnResult -= OnFogResult;
            lootProcessor.OnResult -= OnLootResult;
            gameMapFogProcessor.Dispose();
            mapProcessor.Dispose();
            lootProcessor.Dispose();
            AttackPusher.Dispose();
            FlaskPusher.Dispose();
            SkillPusher.Dispose();
        }

        void OnFogResult(object sender, ImageProcessorEventArgs<GameMapProcessorResult<MapDirectionMoveInfo>> e) {
            time += NativeApiWrapper.StandartDelay;
            var calcRes = e.ImageProcessorResult.VectorizationResult.ToList();
            var itemsToRemove = calcRes.Where(a => a.Angle >= MaxAngle || a.Angle <= MinAngle).ToList();
            foreach (var item in itemsToRemove) {
                calcRes.Remove(item);
            }
            AddToFogHistory(new HistoryElement<MapDirectionMoveInfo>() { MoveInfos = calcRes });
            if (!mapHistory.Any())
                return;
            if (lockObserver.Locked)
                return;
            if (time > MaxAreaTime) {
                OnAreaEnded();
                return;
            }
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
            var point = NativeApiWrapper.GetScreenRotatedPoint((int)mainAngle);
            AvailableInput.MouseMove(point);
            AvailableInput.Input(Settings.MoveKey);
        }
    }
}
