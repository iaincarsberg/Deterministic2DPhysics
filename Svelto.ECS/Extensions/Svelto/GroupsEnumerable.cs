using DBC.ECS;
using Svelto.DataStructures;

namespace Svelto.ECS
{
    /// <summary>
    ///     NOTE THESE ENUMERABLES EXIST TO AVOID BOILERPLATE CODE AS THEY SKIP 0 SIZED GROUPS
    ///     However if the normal pattern with the double foreach is used, this is not necessary
    ///     Note: atm cannot be ref structs because they are returned in a valuetuple
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    public readonly ref struct GroupsEnumerable<T1, T2, T3, T4> where T1 : struct, IEntityComponent
                                                                where T2 : struct, IEntityComponent
                                                                where T3 : struct, IEntityComponent
                                                                where T4 : struct, IEntityComponent
    {
        readonly EntitiesDB                                    _db;
        readonly LocalFasterReadOnlyList<ExclusiveGroupStruct> _groups;

        public GroupsEnumerable(EntitiesDB db, in LocalFasterReadOnlyList<ExclusiveGroupStruct> groups)
        {
            _db     = db;
            _groups = groups;
        }

        public ref struct GroupsIterator
        {
            public GroupsIterator(EntitiesDB db, in LocalFasterReadOnlyList<ExclusiveGroupStruct> groups) : this()
            {
                _groups     = groups;
                _indexGroup = -1;
                _entitiesDB = db;
            }

            public bool MoveNext()
            {
                //attention, the while is necessary to skip empty groups
                while (++_indexGroup < _groups.count)
                {
                    var entityCollection1 = _entitiesDB.QueryEntities<T1, T2, T3>(_groups[_indexGroup]);
                    if (entityCollection1.count == 0)
                        continue;
                    var entityCollection2 = _entitiesDB.QueryEntities<T4>(_groups[_indexGroup]);
                    if (entityCollection2.count == 0)
                        continue;

                    Check.Assert(entityCollection1.count == entityCollection2.count
                               , "congratulation, you found a bug in Svelto, please report it");

                    var array  = entityCollection1;
                    var array2 = entityCollection2;
                    _buffers = new EntityCollection<T1, T2, T3, T4>(array.buffer1, array.buffer2, array.buffer3, array2);
                    break;
                }

                var moveNext = _indexGroup < _groups.count;

                if (moveNext == false)
                    Reset();

                return moveNext;
            }

            public void Reset() { _indexGroup = -1; }

            public RefCurrent<T1, T2, T3, T4> Current => new RefCurrent<T1, T2, T3, T4>(_buffers, _groups[_indexGroup]);

            readonly LocalFasterReadOnlyList<ExclusiveGroupStruct> _groups;

            int                              _indexGroup;
            EntityCollection<T1, T2, T3, T4> _buffers;
            readonly EntitiesDB              _entitiesDB;
        }

        public GroupsIterator GetEnumerator() { return new GroupsIterator(_db, _groups); }
    }
    
    public readonly ref struct GroupsEnumerable<T1, T2, T3, T4, T5> where T1 : struct, IEntityComponent
                                                                where T2 : struct, IEntityComponent
                                                                where T3 : struct, IEntityComponent
                                                                where T4 : struct, IEntityComponent
                                                                where T5 : struct, IEntityComponent
    {
        readonly EntitiesDB                                    _db;
        readonly LocalFasterReadOnlyList<ExclusiveGroupStruct> _groups;

        public GroupsEnumerable(EntitiesDB db, in LocalFasterReadOnlyList<ExclusiveGroupStruct> groups)
        {
            _db     = db;
            _groups = groups;
        }

        public ref struct GroupsIterator
        {
            public GroupsIterator(EntitiesDB db, in LocalFasterReadOnlyList<ExclusiveGroupStruct> groups) : this()
            {
                _groups     = groups;
                _indexGroup = -1;
                _entitiesDB = db;
            }

            public bool MoveNext()
            {
                //attention, the while is necessary to skip empty groups
                while (++_indexGroup < _groups.count)
                {
                    var entityCollection1 = _entitiesDB.QueryEntities<T1, T2, T3>(_groups[_indexGroup]);
                    if (entityCollection1.count == 0)
                        continue;
                    var entityCollection2 = _entitiesDB.QueryEntities<T4, T5>(_groups[_indexGroup]);
                    if (entityCollection2.count == 0)
                        continue;

                    Check.Assert(entityCollection1.count == entityCollection2.count
                               , "congratulation, you found a bug in Svelto, please report it");

                    var array  = entityCollection1;
                    var array2 = entityCollection2;
                    _buffers = new EntityCollection<T1, T2, T3, T4, T5>(array.buffer1, array.buffer2, array.buffer3, array2.buffer1, array2.buffer2);
                    break;
                }

                var moveNext = _indexGroup < _groups.count;

                if (moveNext == false)
                    Reset();

                return moveNext;
            }

            public void Reset() { _indexGroup = -1; }

            public RefCurrent<T1, T2, T3, T4, T5> Current => new RefCurrent<T1, T2, T3, T4, T5>(_buffers, _groups[_indexGroup]);

            readonly LocalFasterReadOnlyList<ExclusiveGroupStruct> _groups;

            int                              _indexGroup;
            EntityCollection<T1, T2, T3, T4, T5> _buffers;
            readonly EntitiesDB              _entitiesDB;
        }

        public GroupsIterator GetEnumerator() { return new GroupsIterator(_db, _groups); }
    }

    public ref struct RefCurrent<T1, T2, T3, T4> where T1 : struct, IEntityComponent
        where T2 : struct, IEntityComponent
        where T3 : struct, IEntityComponent
        where T4 : struct, IEntityComponent
    {
        public RefCurrent(in EntityCollection<T1, T2, T3, T4> buffers, ExclusiveGroupStruct group)
        {
            _buffers = buffers;
            _group   = group;
        }

        public void Deconstruct(out EntityCollection<T1, T2, T3, T4> buffers, out ExclusiveGroupStruct group)
        {
            buffers = _buffers;
            group   = _group;
        }

        public readonly EntityCollection<T1, T2, T3, T4> _buffers;
        public readonly ExclusiveGroupStruct             _group;
    }
    
    public ref struct RefCurrent<T1, T2, T3, T4, T5> where T1 : struct, IEntityComponent
        where T2 : struct, IEntityComponent
        where T3 : struct, IEntityComponent
        where T4 : struct, IEntityComponent
        where T5 : struct, IEntityComponent
    {
        public RefCurrent(in EntityCollection<T1, T2, T3, T4, T5> buffers, ExclusiveGroupStruct group)
        {
            _buffers = buffers;
            _group   = group;
        }

        public void Deconstruct(out EntityCollection<T1, T2, T3, T4, T5> buffers, out ExclusiveGroupStruct group)
        {
            buffers = _buffers;
            group   = _group;
        }

        public readonly EntityCollection<T1, T2, T3, T4, T5> _buffers;
        public readonly ExclusiveGroupStruct             _group;
    }

    /// <summary>
    ///     ToDo source gen could return the implementation of IBuffer directly, but cannot be done manually
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public readonly ref struct GroupsEnumerable<T1, T2, T3> where T1 : struct, IEntityComponent
                                                            where T2 : struct, IEntityComponent
                                                            where T3 : struct, IEntityComponent
    {
        readonly EntitiesDB                                    _db;
        readonly LocalFasterReadOnlyList<ExclusiveGroupStruct> _groups;

        public GroupsEnumerable(EntitiesDB db, in LocalFasterReadOnlyList<ExclusiveGroupStruct> groups)
        {
            _db     = db;
            _groups = groups;
        }

        public ref struct GroupsIterator
        {
            public GroupsIterator(EntitiesDB db, in LocalFasterReadOnlyList<ExclusiveGroupStruct> groups) : this()
            {
                _groups     = groups;
                _indexGroup = -1;
                _entitiesDB = db;
            }

            public bool MoveNext()
            {
                //attention, the while is necessary to skip empty groups
                while (++_indexGroup < _groups.count)
                {
                    var entityCollection = _entitiesDB.QueryEntities<T1, T2, T3>(_groups[_indexGroup]);
                    if (entityCollection.count == 0)
                        continue;

                    _buffers = entityCollection;
                    break;
                }

                var moveNext = _indexGroup < _groups.count;

                if (moveNext == false)
                    Reset();

                return moveNext;
            }

            public void Reset() { _indexGroup = -1; }

            public RefCurrent<T1, T2, T3> Current => new RefCurrent<T1, T2, T3>(_buffers, _groups[_indexGroup]);

            readonly LocalFasterReadOnlyList<ExclusiveGroupStruct> _groups;

            int                          _indexGroup;
            EntityCollection<T1, T2, T3> _buffers;
            readonly EntitiesDB          _entitiesDB;
        }

        public GroupsIterator GetEnumerator() { return new GroupsIterator(_db, _groups); }
    }

    public ref struct RefCurrent<T1, T2, T3> where T1 : struct, IEntityComponent
                                             where T2 : struct, IEntityComponent
                                             where T3 : struct, IEntityComponent
    {
        public RefCurrent(in EntityCollection<T1, T2, T3> buffers, ExclusiveGroupStruct group)
        {
            _buffers = buffers;
            _group   = group;
        }

        public void Deconstruct(out EntityCollection<T1, T2, T3> buffers, out ExclusiveGroupStruct group)
        {
            buffers = _buffers;
            group   = _group;
        }

        public readonly EntityCollection<T1, T2, T3> _buffers;
        public readonly ExclusiveGroupStruct         _group;
    }

    public readonly ref struct GroupsEnumerable<T1, T2>
        where T1 : struct, IEntityComponent where T2 : struct, IEntityComponent
    {
        public GroupsEnumerable(EntitiesDB db, in LocalFasterReadOnlyList<ExclusiveGroupStruct> groups)
        {
            _db     = db;
            _groups = groups;
        }

        public ref struct GroupsIterator
        {
            public GroupsIterator(EntitiesDB db, in LocalFasterReadOnlyList<ExclusiveGroupStruct> groups) : this()
            {
                _db         = db;
                _groups     = groups;
                _indexGroup = -1;
            }

            public bool MoveNext()
            {
                //attention, the while is necessary to skip empty groups
                while (++_indexGroup < _groups.count)
                {
                    var entityCollection = _db.QueryEntities<T1, T2>(_groups[_indexGroup]);
                    if (entityCollection.count == 0)
                        continue;

                    _buffers = entityCollection;
                    break;
                }

                var moveNext = _indexGroup < _groups.count;

                if (moveNext == false)
                    Reset();

                return moveNext;
            }

            public void Reset() { _indexGroup = -1; }

            public RefCurrent<T1, T2> Current => new RefCurrent<T1, T2>(_buffers, _groups[_indexGroup]);

            readonly EntitiesDB                                    _db;
            readonly LocalFasterReadOnlyList<ExclusiveGroupStruct> _groups;

            int                      _indexGroup;
            EntityCollection<T1, T2> _buffers;
        }

        public GroupsIterator GetEnumerator() { return new GroupsIterator(_db, _groups); }

        readonly EntitiesDB                                    _db;
        readonly LocalFasterReadOnlyList<ExclusiveGroupStruct> _groups;
    }

    public ref struct RefCurrent<T1, T2> where T1 : struct, IEntityComponent where T2 : struct, IEntityComponent
    {
        public RefCurrent(in EntityCollection<T1, T2> buffers, ExclusiveGroupStruct group)
        {
            _buffers = buffers;
            _group   = group;
        }

        public void Deconstruct(out EntityCollection<T1, T2> buffers, out ExclusiveGroupStruct group)
        {
            buffers = _buffers;
            group   = _group;
        }

        public readonly EntityCollection<T1, T2> _buffers;
        public readonly ExclusiveGroupStruct     _group;
    }

    public readonly ref struct GroupsEnumerable<T1> where T1 : struct, IEntityComponent
    {
        public GroupsEnumerable(EntitiesDB db, in LocalFasterReadOnlyList<ExclusiveGroupStruct> groups)
        {
            _db     = db;
            _groups = groups;
        }

        public ref struct GroupsIterator
        {
            public GroupsIterator(EntitiesDB db, in LocalFasterReadOnlyList<ExclusiveGroupStruct> groups) : this()
            {
                _db         = db;
                _groups     = groups;
                _indexGroup = -1;
            }

            public bool MoveNext()
            {
                //attention, the while is necessary to skip empty groups
                while (++_indexGroup < _groups.count)
                {
                    var entityCollection = _db.QueryEntities<T1>(_groups[_indexGroup]);
                    if (entityCollection.count == 0)
                        continue;

                    _buffer = entityCollection;
                    break;
                }

                return _indexGroup < _groups.count;
            }

            public void Reset() { _indexGroup = -1; }

            public RefCurrent<T1> Current => new RefCurrent<T1>(_buffer, _groups[_indexGroup]);

            readonly EntitiesDB                                    _db;
            readonly LocalFasterReadOnlyList<ExclusiveGroupStruct> _groups;

            int                  _indexGroup;
            EntityCollection<T1> _buffer;
        }

        public GroupsIterator GetEnumerator() { return new GroupsIterator(_db, _groups); }

        readonly EntitiesDB                                    _db;
        readonly LocalFasterReadOnlyList<ExclusiveGroupStruct> _groups;
    }

    public ref struct RefCurrent<T1> where T1 : struct, IEntityComponent
    {
        public RefCurrent(in EntityCollection<T1> buffers, ExclusiveGroupStruct group)
        {
            _buffers = buffers;
            _group   = group;
        }

        public void Deconstruct(out EntityCollection<T1> buffers, out ExclusiveGroupStruct group)
        {
            buffers = _buffers;
            group   = _group;
        }

        public readonly EntityCollection<T1> _buffers;
        public readonly ExclusiveGroupStruct _group;
    }
}