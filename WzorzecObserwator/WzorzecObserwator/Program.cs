using System;
using System.Collections.Generic;
using System.Threading;

namespace WzorzecObserwator
{
    public interface IEffectObserver
    {
        void Update(EffectType effectType);
    }

    public enum EffectType
    {
        NORMAL,
        SILENCE,
        ROOT,
        BLIND,
        STUN,
        CHARM
    }

    public class Effect
    {
        private EffectType _currentEffect;
        private readonly List<IEffectObserver> _observers;

        public Effect()
        {
            _currentEffect = EffectType.NORMAL;
            _observers = new List<IEffectObserver>();
        }

        public void AddObserver(IEffectObserver observer) => _observers.Add(observer);
        public void RemoveObserver(IEffectObserver observer) => _observers.Remove(observer);

        private void NotifyObservers()
        {
            foreach (var obs in _observers)
                obs.Update(_currentEffect);
        }

        public void TimeDuration()
        {
            foreach (EffectType type in Enum.GetValues(typeof(EffectType)))
            {
                _currentEffect = type;
                Console.WriteLine($"\n[Effect] Current effect changed to: {_currentEffect}");
                NotifyObservers();
                Thread.Sleep(500);
            }
        }
    }

    public class Champions : IEffectObserver
    {
        public void Update(EffectType effectType)
            => Console.WriteLine($"Champion received effect: {effectType}");
    }

    public class Minions : IEffectObserver
    {
        public void Update(EffectType effectType)
            => Console.WriteLine($"Minion received effect: {effectType}");
    }

    public class EpicMonsters : IEffectObserver
    {
        public void Update(EffectType effectType)
            => Console.WriteLine($"EpicMonster received effect: {effectType}");
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var effect = new Effect();
            var champ = new Champions();
            var minion = new Minions();
            var epic = new EpicMonsters();

            effect.AddObserver(champ);
            effect.AddObserver(minion);
            effect.AddObserver(epic);

            effect.TimeDuration();
        }
    }
}
