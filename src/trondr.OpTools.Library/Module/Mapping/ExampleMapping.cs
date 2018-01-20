using System;
using AutoMapper;

namespace trondr.OpTools.Library.Module.Mapping
{
    public class ExampleMapping1 : Profile
    {
        public ExampleMapping1()
        {
            Configure();
        }

        protected void Configure()
        {
            //Configure
            //CreateMap<Foo, Bar>();            
        }
    }
}
