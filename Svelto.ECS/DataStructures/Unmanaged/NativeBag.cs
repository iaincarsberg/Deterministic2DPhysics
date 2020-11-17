#if !DEBUG || PROFILE_SVELTO
#define DISABLE_CHECKS
#endif

#if DEBUG && !PROFILE_SVELTO
//#define ENABLE_DEBUG_CHEKS
#endif

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Svelto.Common;

namespace Svelto.ECS.DataStructures
{
    /// <summary>
    ///     Burst friendly RingBuffer on steroid:
    ///     it can: Enqueue/Dequeue, it wraps if there is enough space after dequeuing
    ///     It resizes if there isn't enough space left.
    ///     It's a "bag", you can queue and dequeue any T. Just be sure that you dequeue what you queue! No check on type
    ///     is done.
    ///     You can reserve a position in the queue to update it later.
    ///     The datastructure is a struct and it's "copyable"
    ///     I eventually decided to call it NativeBag and not NativeBag because it can also be used as
    ///     a preallocated memory pool where any kind of T can be stored as long as T is unmanaged
    /// </summary>
    public struct NativeBag : IDisposable
    {
        public uint count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                unsafe
                {
                    BasicTests();
#if ENABLE_DEBUG_CHEKS                    
                    try
                    {
#endif
                        return _queue->size;
#if ENABLE_DEBUG_CHEKS
                    }
                    finally
                    {
                        Volatile.Write(ref _threadSentinel, 0);
                    }
#endif
                }
            }
        }

        public uint capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                unsafe
                {
                    BasicTests();
#if ENABLE_DEBUG_CHEKS
                    try
                    {
#endif
                        return _queue->capacity;
#if ENABLE_DEBUG_CHEKS
                    }
                    finally
                    {
                        Volatile.Write(ref _threadSentinel, 0);
                    }
#endif
                }
            }
        }

        public NativeBag(Allocator allocator)
        {
            unsafe
            {
                var sizeOf   = MemoryUtilities.SizeOf<UnsafeBlob>();
                var listData = (UnsafeBlob*) MemoryUtilities.Alloc((uint) sizeOf, allocator);

                //clear to nullify the pointers
                //MemoryUtilities.MemClear((IntPtr) listData, (uint) sizeOf);
                listData->allocator = allocator;
                _queue              = listData;
#if ENABLE_DEBUG_CHEKS                
                _threadSentinel     = 0;
#endif                
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEmpty()
        {
            unsafe
            {
                BasicTests();
#if ENABLE_DEBUG_CHEKS
                try
                {
#endif
                    if (_queue == null || _queue->ptr == null)
                        return true;
#if ENABLE_DEBUG_CHEKS
                }
                finally
                {
                    Volatile.Write(ref _threadSentinel, 0);
                }
#endif
            }

            return count == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Dispose()
        {
            if (_queue != null)
            {
#if ENABLE_DEBUG_CHEKS
                //todo: this must be unit tested
                if (Interlocked.CompareExchange(ref _threadSentinel, 1, 0) != 0)
                    throw new Exception("NativeBag is not thread safe, reading and writing operations can happen" +
                        "on different threads, but not simultaneously");

                try
                {
#endif
                    _queue->Dispose();
                    MemoryUtilities.Free((IntPtr) _queue, _queue->allocator);
                    _queue = null;
#if ENABLE_DEBUG_CHEKS
                }
                finally
                {
                    Volatile.Write(ref _threadSentinel, 0);
                }
#endif                
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T ReserveEnqueue<T>(out UnsafeArrayIndex index) where T : struct
        {
            unsafe
            {
                BasicTests();
                
                var sizeOf = MemoryUtilities.SizeOf<T>();
                if (_queue->space - sizeOf < 0)
                    _queue->Realloc((uint) ((_queue->capacity + sizeOf) * 2.0f));

#if ENABLE_DEBUG_CHEKS
                try
                {
#endif                    

                    return ref _queue->Reserve<T>(out index);
#if ENABLE_DEBUG_CHEKS
                }
                finally
                {
                    Volatile.Write(ref _threadSentinel, 0);
                }
#endif
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue<T>(in T item) where T : struct
        {
            unsafe
            {
                BasicTests();

#if ENABLE_DEBUG_CHEKS
                try
                {
#endif
                    var sizeOf = MemoryUtilities.SizeOf<T>();
                    if (_queue->space - sizeOf < 0)
                        _queue->Realloc((uint) ((_queue->capacity + MemoryUtilities.Align4((uint) sizeOf)) * 2.0f));

                    _queue->Write(item);
#if ENABLE_DEBUG_CHEKS
                }
                finally
                {
                    Volatile.Write(ref _threadSentinel, 0);
                }
#endif
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            unsafe
            {
                BasicTests();
#if ENABLE_DEBUG_CHEKS
                try
                {
#endif
                    _queue->Clear();
#if ENABLE_DEBUG_CHEKS
                }
                finally
                {
                    Volatile.Write(ref _threadSentinel, 0);
                }
#endif
            }
        }

        public T Dequeue<T>() where T : struct
        {
            unsafe
            {
                BasicTests();
#if ENABLE_DEBUG_CHEKS
                try
                {
#endif
                    return _queue->Read<T>();
#if ENABLE_DEBUG_CHEKS
                }
                finally
                {
                    Volatile.Write(ref _threadSentinel, 0);
                }
#endif
            }
        }

        internal ref T AccessReserved<T>(UnsafeArrayIndex reserverIndex) where T : struct
        {
            unsafe
            {
                BasicTests();
#if ENABLE_DEBUG_CHEKS                
                try
                {
#endif
                    return ref _queue->AccessReserved<T>(reserverIndex);
#if ENABLE_DEBUG_CHEKS
                }
                finally
                {
                    Volatile.Write(ref _threadSentinel, 0);
                }
#endif
            }
        }

        [Conditional("DISABLE_CHECKS")]
        unsafe void BasicTests()
        {
            if (_queue == null)
                throw new Exception("SimpleNativeArray: null-access");
#if ENABLE_DEBUG_CHEKS
            todo: this must be unit tested
             if (Interlocked.CompareExchange(ref _threadSentinel, 1, 0) != 0)
                 throw new Exception("NativeBag is not thread safe, reading and writing operations can happen"
                                   + "on different threads, but not simultaneously");
#endif            
        }

#if ENABLE_DEBUG_CHEKS
        int _threadSentinel;
#endif
#if UNITY_COLLECTIONS
        [global::Unity.Collections.LowLevel.Unsafe.NativeDisableUnsafePtrRestriction]
#endif
        unsafe UnsafeBlob* _queue;
    }
}