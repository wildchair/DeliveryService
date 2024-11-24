using System.Reflection;

namespace DeliveryService.Utils
{
    public static class PropertyComparator
    {
        /// <summary>
        /// Сравнивает значения одноименных свойств объектов. Типы данных свойств должны быть одинаковы!
        /// </summary>
        /// <typeparam name="T">Тип данных obj1</typeparam>
        /// <typeparam name="D">Тип данных obj2</typeparam>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns>Коллекция разнящихся свойств.</returns>
        public static List<(PropertyInfo original, PropertyInfo changed)> FindChangedProperties<T, D>(T obj1, D obj2)
        {
            var type1 = typeof(T);
            var type2 = typeof(D);

            var properties1 = type1.GetProperties().ToList();
            var properties2 = type2.GetProperties().ToList();

            var generalProperties1 = properties1.IntersectBy(properties2.Select(x => x.Name), x => x.Name).OrderBy(x => x.Name).ToArray();
            var generalProperties2 = properties2.IntersectBy(properties1.Select(x => x.Name), x => x.Name).OrderBy(x => x.Name).ToArray();

            var result = new List<(PropertyInfo, PropertyInfo)>();
            for (int i = 0; i < generalProperties1.Length; i++)
            {
                var a = generalProperties1[i].GetValue(obj1);
                var b = generalProperties2[i].GetValue(obj2);

                if ((a != null && !a.Equals(b)) || (a == null && b != null))
                    result.Add((generalProperties1[i], generalProperties2[i]));
            }

            return result;
        }
    }
}
