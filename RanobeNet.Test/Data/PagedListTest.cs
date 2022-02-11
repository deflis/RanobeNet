using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NUnit.Framework;
using RanobeNet.Data;
using MockQueryable.NSubstitute;

namespace RanobeNet.Test.Data
{
    [TestFixture]
    public class PagedListTest
    {
        [SetUp]
        public void Setup()
        {
        }

        static object[] ParamsOfPagedListAsync =
        {
            new object[] {
                QueryBuilder<MappingSrc>.create(1, 10).SetKeySelector(x => x.Id).SetDescending(false).build(),
                new List<MappingSrc> {
                      new MappingSrc
                    {
                        Id = 1,
                        Name = "1",
                    },
                      new MappingSrc
                    {
                        Id = 4,
                        Name = "2",
                    },
                      new MappingSrc
                    {
                        Id = 3,
                        Name = "3",
                    },
                   new MappingSrc
                    {
                        Id = 2,
                        Name = "4",
                    },
                },
                new List<MappingDest> {
                    new MappingDest
                    {
                        Id = 1,
                    },
                    new MappingDest
                    {
                        Id = 2,
                    },
                    new MappingDest
                    {
                        Id = 3,
                    },
                    new MappingDest
                    {
                        Id = 4,
                    },
                },
                1,
                false,
                false,
            },
            new object[] {
                QueryBuilder<MappingSrc>.create(1, 10).SetKeySelector(x => x.Id).SetDescending(true).build(),
                new List<MappingSrc> {
                      new MappingSrc
                    {
                        Id = 1,
                        Name = "1",
                    },
                      new MappingSrc
                    {
                        Id = 4,
                        Name = "2",
                    },
                      new MappingSrc
                    {
                        Id = 3,
                        Name = "3",
                    },
                   new MappingSrc
                    {
                        Id = 2,
                        Name = "4",
                    },
                },
                new List<MappingDest> {
                    new MappingDest
                    {
                        Id = 4,
                    },
                    new MappingDest
                    {
                        Id = 3,
                    },
                    new MappingDest
                    {
                        Id = 2,
                    },
                    new MappingDest
                    {
                        Id = 1,
                    },
                },
                1,
                false,
                false,
            },
            new object[] {
                QueryBuilder<MappingSrc>.create(1, 10).SetKeySelector(x => x.Name).SetDescending(false).build(),
                new List<MappingSrc> {
                      new MappingSrc
                    {
                        Id = 1,
                        Name = "1",
                    },
                      new MappingSrc
                    {
                        Id = 4,
                        Name = "2",
                    },
                      new MappingSrc
                    {
                        Id = 3,
                        Name = "4",
                    },
                   new MappingSrc
                    {
                        Id = 2,
                        Name = "3",
                    },
                },
                new List<MappingDest> {
                    new MappingDest
                    {
                        Id = 1,
                    },
                    new MappingDest
                    {
                        Id = 4,
                    },
                    new MappingDest
                    {
                        Id = 2,
                    },
                    new MappingDest
                    {
                        Id = 3,
                    },
                },
                1,
                false,
                false,
            },
            new object[] {
                QueryBuilder<MappingSrc>.create(1, 2).SetKeySelector(x => x.Id).SetDescending(false).build(),
                new List<MappingSrc> {
                      new MappingSrc
                    {
                        Id = 1,
                        Name = "1",
                    },
                      new MappingSrc
                    {
                        Id = 4,
                        Name = "2",
                    },
                      new MappingSrc
                    {
                        Id = 3,
                        Name = "4",
                    },
                   new MappingSrc
                    {
                        Id = 2,
                        Name = "3",
                    },
                },
                new List<MappingDest> {
                    new MappingDest
                    {
                        Id = 1,
                    },
                    new MappingDest
                    {
                        Id = 2,
                    },
                },
                2,
                true,
                false,
            },
            new object[] {
                QueryBuilder<MappingSrc>.create(2, 2).SetKeySelector(x => x.Id).SetDescending(false).build(),
                new List<MappingSrc> {
                      new MappingSrc
                    {
                        Id = 1,
                        Name = "1",
                    },
                      new MappingSrc
                    {
                        Id = 4,
                        Name = "2",
                    },
                      new MappingSrc
                    {
                        Id = 3,
                        Name = "4",
                    },
                   new MappingSrc
                    {
                        Id = 2,
                        Name = "3",
                    },
                },
                new List<MappingDest> {
                    new MappingDest
                    {
                        Id = 3,
                    },
                    new MappingDest
                    {
                        Id = 4,
                    },
                },
                2,
                false,
                true,
            },
        };

        [Test]
        [TestCaseSource(nameof(ParamsOfPagedListAsync))]
        public async Task TestToPagedListAsync(Query<MappingSrc> query, List<MappingSrc> src, List<MappingDest> dest, int totalPages, bool hasNext, bool hasPrevious)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MappingSrc, MappingDest>();
            });
            var mapper = new Mapper(config);

            var pagedList = await src.ToList().AsQueryable().BuildMock().ToPagedListAsync<MappingSrc, MappingDest>(query, mapper);

            Assert.AreEqual(pagedList.TotalCount, src.Count());
            Assert.AreEqual(pagedList.PageSize, query.PageSize);
            Assert.AreEqual(pagedList.CurrentPage, query.PageNumber);
            Assert.AreEqual(pagedList.TotalPages, totalPages);
            Assert.AreEqual(pagedList.HasNext, hasNext);
            Assert.AreEqual(pagedList.HasPrevious, hasPrevious);
            CollectionAssert.AreEqual(pagedList.Items, dest);
        }

        public class MappingSrc
        {
            public long Id { get; set; }
            public string Name { get; set; } = null!;
        }
        public class MappingDest : IEquatable<MappingDest>
        {
            public long Id { get; set; }

            public bool Equals(MappingDest? other)
            {
                if (other == null)
                {
                    return false;
                }

                return this.Id == other.Id;
            }

            public override bool Equals(object? other)
            {
                return this.Equals(other as MappingDest);
            }

            public override int GetHashCode()
            {
                return this.Id.GetHashCode();
            }
        }
    }
}
