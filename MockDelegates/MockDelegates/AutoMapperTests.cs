using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MockDelegates
{
    [TestClass]
    public class AutoMapperTests
    {
        [TestMethod]
        public void TestAutomapper1()
        {
//Create an implementation of auto mapper IMapper
var config = new MapperConfiguration(
    cfg =>
    {
        cfg.CreateMap<Order, OrderDto>();
        cfg.CreateMap<Person, PersonDto>();
    }
    );
var mapper = config.CreateMapper();

//Register serviceas
var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton(mapper);
serviceCollection.AddSingleton<AutoMapperWrapper>();            
var serviceProvider = serviceCollection.BuildServiceProvider();

//Use the IoC container to construct our class and inject dependencies for us
var mapperWrapper = serviceProvider.GetService<AutoMapperWrapper>();

//Perfom mapping
var order = new Order();
var person = new Person();
var orderDto = mapperWrapper.Map<OrderDto>(order);
var personDto = mapperWrapper.Map<PersonDto>(person);

            Assert.AreEqual(order.Id, orderDto.Id);
            Assert.AreEqual(person.Id, personDto.Id);
        }

        [TestMethod]
        public void TestAutomapper2()
        {
            //Create an implementation of auto mapper IMapper
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Order, OrderDto>();
                    cfg.CreateMap<Person, PersonDto>();
                }
                );
            var mapper = config.CreateMapper();

            //Register serviceas
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(mapper);
            serviceCollection.AddSingleton<Map<Person>>(sp => sp.GetRequiredService<AutoMapperWrapper>().Map<Person>);
            serviceCollection.AddSingleton<Map<Order>>(sp => sp.GetRequiredService<AutoMapperWrapper>().Map<Order>);
            serviceCollection.AddSingleton<AutoMapperWrapper>();

            //Get the injected service
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mapPerson = serviceProvider.GetService<Map<Person>>();
            var mapOrder = serviceProvider.GetService<Map<Order>>();

            //Perfom mapping
            var order = new Order();
            var person = new Person();
            var orderDto = mapOrder(order);
            var personDto = mapPerson(person);

            Assert.AreEqual(order.Id, orderDto.Id);
            Assert.AreEqual(person.Id, personDto.Id);
        }
    }



    public delegate TDestination Map<TDestination>(object source);

    public class AutoMapperWrapper
    {
        IMapper _mapper;

        public AutoMapperWrapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }
    }
}
