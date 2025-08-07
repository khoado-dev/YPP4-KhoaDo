namespace SqlCoreByHand
{
    public enum JoinType
    {
        Inner,
        Left,
        Right,
        Full,
        Cross
    }

    public enum OperationType
    {
        Sum,
        Avg,
        Min,
        Max,
        Count
    }

    public class SqlService<L, R>
    {
        private readonly Dictionary<string, Func<L, R, bool>> _joinConditions = new();

        public void RegisterJoinCondition(string key, Func<L, R, bool> predicate)
        {
            _joinConditions[key] = predicate;
        }

        public void RemoveJoinCondition(string key)
        {
            if (_joinConditions.ContainsKey(key))
                _joinConditions.Remove(key);
        }

        /// <summary>
        /// Join types: inner, left, right, full, cross
        /// </summary>
        public List<(L?, R?)> Join(List<L> left, List<R> right, JoinType joinType, string conditionKey = "")
        {
            var result = new List<(L?, R?)>();

            if (joinType == JoinType.Cross)
            {
                foreach (var l in left)
                    foreach (var r in right)
                        result.Add((l, r));
                return result;
            }

            if (!_joinConditions.ContainsKey(conditionKey))
                throw new ArgumentException($"Condition '{conditionKey}' not found.");

            var condition = _joinConditions[conditionKey];

            var matchedLefts = new HashSet<int>();
            var matchedRights = new HashSet<int>();

            // Always check for matches first
            for (int i = 0; i < left.Count; i++)
            {
                for (int j = 0; j < right.Count; j++)
                {
                    if (condition(left[i], right[j]))
                    {
                        result.Add((left[i], right[j]));
                        matchedLefts.Add(i);
                        matchedRights.Add(j);
                    }
                }
            }

            // Add unmatched LEFT
            if (joinType == JoinType.Left || joinType == JoinType.Full)
            {
                for (int i = 0; i < left.Count; i++)
                {
                    if (!matchedLefts.Contains(i))
                    {
                        result.Add((left[i], default));
                    }
                }
            }

            // Add unmatched RIGHT
            if (joinType == JoinType.Right || joinType == JoinType.Full)
            {
                for (int j = 0; j < right.Count; j++)
                {
                    if (!matchedRights.Contains(j))
                    {
                        result.Add((default, right[j]));
                    }
                }
            }

            return result;
        }


        public List<L> Where(List<L> source, Func<L, bool> predicate)
        {
            var result = new List<L>();
            foreach (var item in source)
                if (predicate(item))
                    result.Add(item);
            return result;
        }

        public int Aggregate(List<L> source, Func<L, int> selector, OperationType operation)
        {
            if (source == null || source.Count == 0) return 0;

            int result = 0;
            switch (operation)
            {
                case OperationType.Sum:
                    foreach (var item in source)
                        result += selector(item);
                    break;

                case OperationType.Avg:
                    int sum = 0, count = 0;
                    foreach (var item in source)
                    {
                        sum += selector(item);
                        count++;
                    }
                    result = count > 0 ? sum / count : 0;
                    break;

                case OperationType.Min:
                    result = selector(source[0]);
                    foreach (var item in source)
                    {
                        int value = selector(item);
                        if (value < result)
                            result = value;
                    }
                    break;

                case OperationType.Max:
                    result = selector(source[0]);
                    foreach (var item in source)
                    {
                        int value = selector(item);
                        if (value > result)
                            result = value;
                    }
                    break;

                case OperationType.Count:
                    result = source.Count;
                    break;

                default:
                    throw new ArgumentException("Unsupported aggregate operation");
            }

            return result;
        }
    }
}
