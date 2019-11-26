using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Model
{
    public class StaticSettings
    {

        public bool IsShowFoolChanges = false;

        /// <summary>
        /// Максимальный уровень персонажа
        /// </summary>
        public int MaxPersLevel=100;

        /// <summary>
        /// Максимальный уровень для задач и характеристик
        /// </summary>
        public int MaxAbChaLevel=10;

        /// <summary>
        /// Максимальное возможное значение для повторяющихся задач
        /// </summary>
        public double MaxTaskValue=55.0;


        /// <summary>
        /// Уровней скиллов до уровня характеристики
        /// </summary>
        public double AbLevelsToChaLevel = 5.0;

        /// <summary>
        /// Уровней характеристик до уровня персонажа
        /// </summary>
        public double ChaLevelsToPersLevel = 3.0;

        /// <summary>
        /// Число дней для первого уровня скилла
        /// </summary>
        public int AbOneLevelDays = 2;

        /// <summary>
        /// Число очков скиллов на первом уровне персонажа
        /// </summary>
        public int AbPointsFirstLevel = 30;

        /// <summary>
        /// Очков скиллов за 1 уровень персонажа
        /// </summary>
        public int AbPointsOneLevel = 10;

        /// <summary>
        /// Экспонента для расчета опыта, чем больше, тем сложнее зарабатываются уровни
        /// </summary>
        public  double ExpExponenta = 1.5;

        /// <summary>
        /// Экспонента для бонусов за скиллы
        /// </summary>
        public double AbExpExponenta = 1.15;

        /// <summary>
        /// Опыта за задачу, влияющую на простой скилл
        /// </summary>
        public double ExpEasyAb = 100.0;

        /// <summary>
        /// Делитель для простых задач скилла. Чем меньше, тем больше опыта.
        /// </summary>
        public double AbTaskDelay = 0.5;

        /// <summary>
        /// Опыта за самый простой квест
        /// </summary>
        public  double QwestExp = 625.0;

        /// <summary>
        /// Опыта за просто задачу
        /// </summary>
        public  int ExpToTask = 25;

        /// <summary>
        /// Плюс к здоровью за 1 уровень
        /// </summary>
        public  int PlusHPOneLevel = 10;

        public double BuffTaskValue = 0.33;

        public double BuffTaskExp = 0.33;


    }
}
