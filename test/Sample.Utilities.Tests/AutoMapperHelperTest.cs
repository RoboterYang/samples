using System;
using Xunit;
using Sample.Utilities;
using AutoMapper;

namespace Sample.Utilities.Tests
{
    [Trait("������", "����ӳ��")]
    public class AutoMapperHelperTest
    {
        public AutoMapperHelperTest()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SourceClass, DestinationClass>();
            });
        }
        [Fact(DisplayName = "�������")]
        public void AutoMapperHelper_WithExpectedParameters()
        {
            // arrange
            var src = new SourceClass { Id = 1 };

            // act
            var dest = AutoMapperHelper.MapTo<DestinationClass>(src);

            // assert
            Assert.NotNull(dest);
            Assert.True(AreEqualsObjectValues(src, dest));
        }

        #region private function
        private bool AreEqualsObjectValues(SourceClass src, DestinationClass dest)
        {
            return src.Id == dest.Id;
        }
        #endregion
    }


    class SourceClass
    {
        public int Id { get; set; }
    }
    class DestinationClass
    {
        public int Id { get; set; }
    }
}
