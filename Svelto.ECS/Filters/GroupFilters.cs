using Svelto.DataStructures;

namespace Svelto.ECS
{
    public struct GroupFilters
    {
        internal GroupFilters(SharedSveltoDictionaryNative<int, FilterGroup> filters, ExclusiveGroupStruct group)
        {
            this.filters = filters;
            _group = @group;
        }

        public ref FilterGroup GetFilter(int filterIndex)
        {
#if DEBUG && !PROFILE_SVELTO
            if (filters.isValid == false)
                throw new ECSException($"trying to fetch not existing filters {filterIndex} group {_group.ToName()}");
            if (filters.ContainsKey(filterIndex) == false)
                throw new ECSException($"trying to fetch not existing filters {filterIndex} group {_group.ToName()}");
#endif
            return ref filters.GetValueByRef(filterIndex);
        }

        public bool HasFilter(int filterIndex)
        {
            return filters.ContainsKey(filterIndex);
        }

        public void ClearFilter(int filterIndex)
        {
            if (filters.TryFindIndex(filterIndex, out var index))
                filters.GetValues(out _)[index].Clear();
        }
        
        public void ClearFilters()
        {
            foreach (var filter in filters)
                filter.Value.Clear();
        }
        
        public void DeleteFilters()
        {
            foreach (var filter in filters)
                filter.Value.Clear();
            
            filters.FastClear();
        }

        public bool TryGetFilter(int filterIndex, out FilterGroup filter)
        {
            return filters.TryGetValue(filterIndex, out filter);
        }

        public SveltoDictionary<int, FilterGroup, NativeStrategy<FasterDictionaryNode<int>>, NativeStrategy<FilterGroup>, NativeStrategy<int>>.SveltoDictionaryKeyValueEnumerator GetEnumerator()
        {
            return filters.GetEnumerator();
        }

        public ref FilterGroup CreateOrGetFilter(int filterID)
        {
            if (filters.TryFindIndex(filterID, out var index) == false)
            {
                var orGetFilterForGroup = new FilterGroup(_group);
                
                filters[filterID] = orGetFilterForGroup;

                return ref filters.GetValueByRef(filterID);
            }

            return ref filters.GetValues(out _)[index];
        }

        internal void Dispose()
        {
            foreach (var filter in filters)
            {
                filter.Value.Dispose();
            }

            filters.Dispose();
        }
        
        readonly ExclusiveGroupStruct _group;
        SharedSveltoDictionaryNative<int, FilterGroup> filters;
    }
}