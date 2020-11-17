using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Svelto.Common;
using Svelto.ECS;
using Svelto.ECS.DataStructures;

namespace Svelto.ECS.Tests.Native
{
    [TestFixture]
    public class NativeArrayTests
    {
        [Test]
        public void TestByteReallocWorks()
        {
            using (var _simpleNativeBag = NativeDynamicArray.Alloc<uint>(Allocator.Temp))
            {
                for (var i = 0; i < 33; i++)
                    _simpleNativeBag.Add<uint>((byte) 0);

                Assert.That(_simpleNativeBag.Count<uint>(), Is.EqualTo(33));
            }
        }

        [Test]
        public void TestSetOutOfTheIndex()
        {
            using (var _simpleNativeBag = NativeDynamicArray.Alloc<uint>(Allocator.Temp, 20))
            {
                _simpleNativeBag.Set<uint>(10, 10);

                Assert.That(_simpleNativeBag.Get<uint>(10), Is.EqualTo(10));
                Assert.That(_simpleNativeBag.Count<uint>(), Is.EqualTo(11));
            }
        }

        [Test]
        public void TestSetOutOfTheCapacity()
        {
            using (var _simpleNativeBag = NativeDynamicArray.Alloc<uint>(Allocator.Temp))
            {
                Assert.Throws<Exception>(() => _simpleNativeBag.Set<uint>(10, 10));
            }
        }
    }

    [TestFixture]
    public class MemoryUtilitiesTests
    {
        struct Test
        {
            public int a;
            public int b;
        }

        [Test]
        public void TestResize()
        {
            unsafe
            {
                var ptr = MemoryUtilities.Alloc(10, Allocator.Persistent);
                Unsafe.Write((void*) ptr, new Test()
                {
                    a = 3
                  , b = 1
                });
                Unsafe.Write((void*) (ptr + 8), (short) -10);
                ptr = MemoryUtilities.Realloc(ptr, 10, 16, Allocator.Persistent);
                var test = Unsafe.Read<Test>((void*) ptr);
                Assert.That(test.a == 3);
                Assert.That(test.b == 1);
                Assert.That(Unsafe.Read<short>((void*) (ptr + 8)) == -10);
                MemoryUtilities.Free(ptr, Allocator.Persistent);
            }
        }
    }

    [TestFixture]
    public class NativeBagTests
    {
        [SetUp]
        public void Init() { }

        [Test]
        public void TestByteReallocWorks()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                for (var i = 0; i < 32; i++)
                    _simpleNativeBag.Enqueue((byte) 0);

                Assert.That(_simpleNativeBag.count, Is.EqualTo(128));
            }
        }

        [Test]
        public void TestCantDequeMoreThanQueue()
        {
            Assert.Throws<Exception>(() =>
            {
                using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
                {
                    _simpleNativeBag.Enqueue((byte) 0);
                    _simpleNativeBag.Enqueue((byte) 0);

                    for (var i = 0; i < 3; i++)
                    {
                        _simpleNativeBag.Enqueue((byte) 0);
                        _simpleNativeBag.Dequeue<byte>();
                        _simpleNativeBag.Dequeue<byte>();
                    }

                    Assert.That(_simpleNativeBag.count, Is.EqualTo(32));
                }
            });
        }

        [Test]
        public void TestCaseReaderGreaterThanWriter() { }

        [Test]
        public void TestDoofusesScenario()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                for (var i = 0; i < 32; i++)
                {
                    _simpleNativeBag.Enqueue((uint) i);
                    _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                }

                var index = 0;

                while (_simpleNativeBag.IsEmpty() == false)
                {
                    Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(index));
                    var dequeue = _simpleNativeBag.Dequeue<EGID>();
                    index++;
                    Assert.That(_simpleNativeBag.count == 32 * 12 - index * 12);
                    Assert.That(dequeue.entityID, Is.EqualTo(1));
                    Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                }
            }
        }

        [Test]
        public void TestDoofusesScenario2()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                var dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
            }
        }

        [Test]
        public void TestDoofusesScenario3()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                var dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));

                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));

                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct()));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(0));
            }
        }

        struct ExclusiveGroupStruct
        {
            internal uint group;
            public ExclusiveGroupStruct(uint i) { @group = i; }
        }

        struct EGID
        {
            internal uint entityID;
            internal uint groupID;

            public EGID(uint i, ExclusiveGroupStruct exclusiveGroupStruct)
            {
                entityID = i;
                @groupID = exclusiveGroupStruct.group;
            }
        }

        [Test]
        public void TestDoofusesScenario4()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(1)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(2)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(3)));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                var dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(1));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(2));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(3));

                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(1)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(2)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(3)));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(1));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(2));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(3));

                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(1)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(2)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(3)));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(1));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(2));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(3));
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct EGIDU
        {
            internal uint         entityID;
            ExclusiveGroupStructU exclusiveGroupStruct;
            internal short        groupID => exclusiveGroupStruct.group;

            public EGIDU(uint i, ExclusiveGroupStructU exclusiveGroupStruct)
            {
                entityID                  = i;
                this.exclusiveGroupStruct = exclusiveGroupStruct;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ExclusiveGroupStructU
        {
            internal short group;
            byte           test;
            public ExclusiveGroupStructU(uint i) : this() { @group = (short) i; }
        }

        [Test]
        public void TestDoofusesScenario4Unaligned()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGIDU(1, new ExclusiveGroupStructU(1)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGIDU(1, new ExclusiveGroupStructU(2)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGIDU(1, new ExclusiveGroupStructU(3)));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                var dequeue = _simpleNativeBag.Dequeue<EGIDU>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(1));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGIDU>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(2));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGIDU>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(3));

                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGIDU(1, new ExclusiveGroupStructU(1)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGIDU(1, new ExclusiveGroupStructU(2)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGIDU(1, new ExclusiveGroupStructU(3)));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGIDU>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(1));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGIDU>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(2));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGIDU>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(3));

                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGIDU(1, new ExclusiveGroupStructU(1)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGIDU(1, new ExclusiveGroupStructU(2)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGIDU(1, new ExclusiveGroupStructU(3)));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGIDU>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(1));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGIDU>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(2));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGIDU>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(3));
            }
        }

        [Test]
        public void TestDoofusesScenario5()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(1)));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                var dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(1));

                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(2)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(3)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(4)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(5)));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(2));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(3));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(4));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(5));

                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(6)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(7)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(8)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(9)));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(6));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(7));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(8));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(9));

                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(10)));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(10));

                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(11)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(12)));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(11));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(12));

                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(13)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(14)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(15)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(16)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(17)));
                _simpleNativeBag.Enqueue((uint) 1);
                _simpleNativeBag.Enqueue(new EGID(1, new ExclusiveGroupStruct(18)));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(13));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(14));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(15));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(16));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(17));

                Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(1));
                dequeue = _simpleNativeBag.Dequeue<EGID>();
                Assert.That(dequeue.entityID, Is.EqualTo(1));
                Assert.That((uint) dequeue.groupID, Is.EqualTo(18));
            }
        }

        [Test]
        public void TestEnqueueDequeueWontAlloc()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                for (var i = 0; i < 32; i++)
                {
                    _simpleNativeBag.Enqueue((byte) 0);
                    _simpleNativeBag.Dequeue<byte>();
                }

                Assert.That(_simpleNativeBag.capacity, Is.EqualTo(8));
            }
        }

        [Test]
        public void TestEnqueueDequeueWontAllocTooMuch()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                for (var i = 0; i < 32; i++)
                {
                    _simpleNativeBag.Enqueue((byte) 0);
                    _simpleNativeBag.Enqueue((byte) 0);
                    _simpleNativeBag.Enqueue((byte) 0);
                    _simpleNativeBag.Enqueue((byte) 0);
                    _simpleNativeBag.Dequeue<byte>();
                    _simpleNativeBag.Dequeue<byte>();
                    _simpleNativeBag.Dequeue<byte>();
                    _simpleNativeBag.Dequeue<byte>();
                }

                Assert.That(_simpleNativeBag.capacity, Is.EqualTo(24));
            }
        }

        [Test]
        public void TestEnqueueDequeueWontAllocTooMuchOnce()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                {
                    _simpleNativeBag.Enqueue((byte) 0);
                    Assert.That(_simpleNativeBag.capacity, Is.EqualTo(8));
                    _simpleNativeBag.Enqueue((byte) 0);
                    Assert.That(_simpleNativeBag.capacity, Is.EqualTo(8));
                    _simpleNativeBag.Enqueue((byte) 0);
                    Assert.That(_simpleNativeBag.capacity, Is.EqualTo(24)); //(8 + 1) * 2 = 18 => aligned 24
                    _simpleNativeBag.Enqueue((byte) 0);
                    Assert.That(_simpleNativeBag.capacity, Is.EqualTo(24));
                    _simpleNativeBag.Dequeue<byte>();
                    _simpleNativeBag.Dequeue<byte>();
                    _simpleNativeBag.Dequeue<byte>();
                    _simpleNativeBag.Dequeue<byte>();
                }

                Assert.That(_simpleNativeBag.capacity, Is.EqualTo(24));
            }
        }

        struct Weird
        {
            public short a;
            public byte  b;
        }

        [Test]
        public void TestEnqueueDequeueWontAllocTooMuchWithWeirdStruct()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                for (var i = 0; i < 32; i++)
                {
                    _simpleNativeBag.Enqueue(new Weird()); //8
                    _simpleNativeBag.Enqueue(new Weird()); //OK
                    _simpleNativeBag.Enqueue(new Weird()); //24
                    _simpleNativeBag.Enqueue(new Weird()); //ok
                    _simpleNativeBag.Dequeue<Weird>();
                    _simpleNativeBag.Dequeue<Weird>();
                    _simpleNativeBag.Dequeue<Weird>();
                    _simpleNativeBag.Dequeue<Weird>();
                }

                Assert.That(_simpleNativeBag.capacity, Is.EqualTo(24));
            }
        }

        [Test]
        public void TestEnqueueDequeueWontAllocTooMuchWithWeirdStructOnce()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                {
                    _simpleNativeBag.Enqueue(new Weird()); //8
                    Assert.That(_simpleNativeBag.capacity, Is.EqualTo(8));
                    _simpleNativeBag.Enqueue(new Weird()); //OK
                    Assert.That(_simpleNativeBag.capacity, Is.EqualTo(8));
                    _simpleNativeBag.Enqueue(new Weird()); //24
                    Assert.That(_simpleNativeBag.capacity, Is.EqualTo(24));
                    _simpleNativeBag.Enqueue(new Weird()); //ok
                    _simpleNativeBag.Dequeue<Weird>();
                    _simpleNativeBag.Dequeue<Weird>();
                    _simpleNativeBag.Dequeue<Weird>();
                    _simpleNativeBag.Dequeue<Weird>();
                }

                Assert.That(_simpleNativeBag.capacity, Is.EqualTo(24));
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct Weird2
        {
            public short a;
            public byte  b;
        }

        [Test]
        public void TestEnqueueDequeueWontAllocTooMuchWithWeirdStructUnalignedOnce()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                {
                    _simpleNativeBag.Enqueue(new Weird2()); //8
                    Assert.That(_simpleNativeBag.capacity, Is.EqualTo(8));
                    _simpleNativeBag.Enqueue(new Weird2()); //OK
                    Assert.That(_simpleNativeBag.capacity, Is.EqualTo(8));
                    _simpleNativeBag.Enqueue(new Weird2()); //24
                    Assert.That(_simpleNativeBag.capacity, Is.EqualTo(24));
                    _simpleNativeBag.Enqueue(new Weird2()); //ok
                    _simpleNativeBag.Dequeue<Weird2>();
                    _simpleNativeBag.Dequeue<Weird2>();
                    _simpleNativeBag.Dequeue<Weird2>();
                    _simpleNativeBag.Dequeue<Weird2>();
                }

                Assert.That(_simpleNativeBag.capacity, Is.EqualTo(24));
            }
        }

        [Test]
        public void TestEnqueueTwiceDequeueOnceLeavesWithHalfOfTheEntities()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                for (var i = 0; i < 32; i++)
                {
                    _simpleNativeBag.Enqueue((byte) 0);
                    _simpleNativeBag.Enqueue((byte) 0);
                    _simpleNativeBag.Dequeue<byte>();
                }

                Assert.That(_simpleNativeBag.count, Is.EqualTo(208));
            }
        }

        [Test]
        public void TestLongReallocWorks()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                for (var i = 0; i < 32; i++)
                    _simpleNativeBag.Enqueue((long) 0);

                Assert.That(_simpleNativeBag.count, Is.EqualTo(32 * 8));
            }
        }

        [Test]
        public void TestMixedReallocWorks()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                for (var i = 0; i < 2; i++)
                {
                    _simpleNativeBag.Enqueue((byte) 0);
                    _simpleNativeBag.Enqueue((uint) 0);
                    _simpleNativeBag.Enqueue((long) 0);
                }

                Assert.That(_simpleNativeBag.count, Is.EqualTo(32));
            }
        }

        [Test]
        public void TestUintReallocWorks()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                for (var i = 0; i < 32; i++)
                    _simpleNativeBag.Enqueue((uint) 0);

                Assert.That(_simpleNativeBag.count, Is.EqualTo(32 * 4));
            }
        }

        [Test]
        public void TestWhatYouEnqueueIsWhatIDequeue()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                for (var i = 0; i < 32; i++)
                {
                    _simpleNativeBag.Enqueue((byte) 0);
                    _simpleNativeBag.Enqueue((byte) 1);
                    _simpleNativeBag.Enqueue((byte) 2);
                    _simpleNativeBag.Enqueue((byte) 3);
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(0));
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(1));
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(2));
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(3));
                }

                Assert.That(_simpleNativeBag.capacity, Is.EqualTo(24));
            }
        }

        [Test]
        public void TestWhatYouEnqueueIsWhatIDequeueMixed()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                for (var i = 0; i < 32; i++)
                {
                    _simpleNativeBag.Enqueue((byte) 0); //4
                    _simpleNativeBag.Enqueue(new Weird2()
                    {
                        a = 0xFA
                      , b = 7
                    }); //(4 + 4) * 2 = 16
                    _simpleNativeBag.Enqueue(new Weird()
                    {
                        a = 0xFA
                      , b = 7
                    });
                    _simpleNativeBag.Enqueue(new Weird2()
                    {
                        a = 0xFA
                      , b = 7
                    });
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(0));
                    Assert.That(_simpleNativeBag.Dequeue<Weird2>(), Is.EqualTo(new Weird2()
                    {
                        a = 0xFA
                      , b = 7
                    }));
                    Assert.That(_simpleNativeBag.Dequeue<Weird>(), Is.EqualTo(new Weird()
                    {
                        a = 0xFA
                      , b = 7
                    }));
                    Assert.That(_simpleNativeBag.Dequeue<Weird2>(), Is.EqualTo(new Weird2()
                    {
                        a = 0xFA
                      , b = 7
                    }));
                }

                Assert.That(_simpleNativeBag.capacity, Is.EqualTo(24));
            }
        }

        [Test]
        public void TestWhatYouEnqueueIsWhatIDequeue2()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                for (var i = 0; i < 32; i++)
                {
                    _simpleNativeBag.Enqueue((byte) 0);
                    _simpleNativeBag.Enqueue((byte) 1);
                    _simpleNativeBag.Enqueue((byte) 2);
                    _simpleNativeBag.Enqueue((byte) 3);
                }

                for (var i = 0; i < 32; i++)
                {
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(0));
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(1));
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(2));
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(3));
                }

                for (var i = 0; i < 32; i++)
                {
                    _simpleNativeBag.Enqueue((byte) 0);
                    _simpleNativeBag.Enqueue((byte) 1);
                    _simpleNativeBag.Enqueue((byte) 2);
                    _simpleNativeBag.Enqueue((byte) 3);
                }

                for (var i = 0; i < 32; i++)
                {
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(0));
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(1));
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(2));
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(3));
                }

                for (var i = 0; i < 32; i++)
                {
                    _simpleNativeBag.Enqueue((byte) 0);
                    _simpleNativeBag.Enqueue((byte) 1);
                    _simpleNativeBag.Enqueue((byte) 2);
                    _simpleNativeBag.Enqueue((byte) 3);
                }

                for (var i = 0; i < 32; i++)
                {
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(0));
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(1));
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(2));
                    Assert.That(_simpleNativeBag.Dequeue<byte>(), Is.EqualTo(3));
                }

                Assert.That(_simpleNativeBag.capacity, Is.EqualTo(1016));
            }
        }

        [Test]
        public void TestReaderGreaterThanWriter()
        {
            using (var _simpleNativeBag = new NativeBag(Allocator.Temp))
            {
                //write 16 uint. The writerHead will be at the end of the array
                for (var i = 0; i < 16; i++)
                {
                    _simpleNativeBag.Enqueue((uint) i);
                }

                Assert.That(_simpleNativeBag.count, Is.EqualTo(64));
                Assert.That(_simpleNativeBag.capacity, Is.EqualTo(120)); //

                //read 8 uint, the readerHead will be in the middle of the array
                for (var i = 0; i < 8; i++)
                {
                    Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(i));
                }

                Assert.That(_simpleNativeBag.count, Is.EqualTo(32));
                Assert.That(_simpleNativeBag.capacity, Is.EqualTo(120));

                //write 4 uint, now the writer head wrapped and it's before the reader head
                //capacity must stay unchanged
                for (var i = 16; i < 16 + 7; i++)
                {
                    _simpleNativeBag.Enqueue((uint) i);
                }

                Assert.That(_simpleNativeBag.count, Is.EqualTo(60));
                Assert.That(_simpleNativeBag.capacity, Is.EqualTo(120));

                //now I will surpass reader, so it won't change the capacity because there is enough space
                for (var i = 16 + 7; i < 16 + 7 + 2; i++)
                {
                    _simpleNativeBag.Enqueue((uint) i);
                }

                Assert.That(_simpleNativeBag.count, Is.EqualTo(68));
                Assert.That(_simpleNativeBag.capacity, Is.EqualTo(120));

                //dequeue everything and verify values
                int index = 8;
                while (_simpleNativeBag.IsEmpty())
                {
                    Assert.That(_simpleNativeBag.Dequeue<uint>(), Is.EqualTo(index));
                    index++;
                }
            }
        }
    }
}