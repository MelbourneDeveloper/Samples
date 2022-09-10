import 'package:flutter/material.dart';

//---Utility classes that will go into a package-------------

///This gives your view model, business logic, or whatever you want to call it,
///access to setState() like the State class does.
class Bloobit<TState> {
  Bloobit(this.initialState) : _state = initialState;

  late final void Function(VoidCallback fn)? _setState;
  final TState initialState;
  TState _state;

  TState get state => _state;

  ///Pass in the setState function from your state here
  void attach(Function(VoidCallback fn) setState) => _setState = setState;

  void emit(TState state) {
    assert(_setState != null, 'You must call attach');
    _setState!(() {
      _state = state;
    });
  }
}

///Extends the State class to automate the attach method
mixin AttachesSetState<TWidget extends StatefulWidget,
    TCallsSetState extends Bloobit> on State<TWidget> {
  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    SetStatePropagator.of<TCallsSetState>(context).stateSetter.attach(setState);
  }
}

///This inherited widget propagates the view model down the widget tree
///much like Provider
class SetStatePropagator<T extends Bloobit> extends InheritedWidget {
  const SetStatePropagator({
    required this.stateSetter,
    required Widget child,
    final Key? key,
  }) : super(key: key, child: child);

  final T stateSetter;

  static SetStatePropagator<T> of<T extends Bloobit>(BuildContext context) {
    final result =
        context.dependOnInheritedWidgetOfExactType<SetStatePropagator<T>>();
    assert(result != null, 'No class of type $T found in context');

    return result!;
  }

  @override
  bool updateShouldNotify(covariant final InheritedWidget oldWidget) =>
      stateSetter != (oldWidget as SetStatePropagator<T>).stateSetter;
}
//---Utility classes that will go into a package-------------

//---Classes specific to this app----------------------------

class AppViewModelState {
  final int callCount;
  final bool isProcessing;
  final bool displayWidgets;

  AppViewModelState(this.callCount, this.isProcessing, this.displayWidgets);

  AppViewModelState copyWith({
    int? callCount,
    bool? isProcessing,
    bool? displayWidgets,
  }) =>
      AppViewModelState(
        callCount ?? this.callCount,
        isProcessing ?? this.isProcessing,
        displayWidgets ?? this.displayWidgets,
      );
}

///This app's view model. This basically works like a Cubit
///however, you call setState() instead of emit() and
///you set the state inside the callback as the framework
///recommends
class AppViewModel extends Bloobit<AppViewModelState> {
  int get callCount => state.callCount;
  bool get isProcessing => state.isProcessing;
  bool get displayWidgets => state.displayWidgets;

  final CountServerService countServerService;

  AppViewModel(this.countServerService)
      : super(AppViewModelState(0, false, true));

  void hideWidgets() {
    emit(state.copyWith(displayWidgets: false));
  }

  Future<void> callGetCount() async {
    emit(state.copyWith(isProcessing: true));

    final callCount = await countServerService.getCallCount();

    emit(state.copyWith(isProcessing: false, callCount: callCount));
  }
}

///This emulates a server that counts the number of times it has been called.
class CountServerService {
  int _counter = 0;
  Future<int> getCallCount() =>
      Future<int>.delayed(const Duration(seconds: 1), () async {
        _counter++;
        return _counter;
      });
}
//---Classes specific to this app----------------------------

void main() {
  //Register services and the view model with an IoC container
  final builder = IocContainerBuilder()
    ..addSingletonService(CountServerService())
    ..addSingleton(
        (container) => AppViewModel(container.get<CountServerService>()));

  runApp(MyApp(builder.toContainer()));
}

class MyApp extends StatelessWidget {
  final IocContainer container;

  const MyApp(this.container, {super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: SetStatePropagator<AppViewModel>(
        stateSetter: container.get<AppViewModel>(),
        child: const Home(),
      ),
    );
  }
}

class Home extends StatefulWidget {
  const Home({super.key});

  @override
  State<Home> createState() => _HomeState();
}

class _HomeState extends
    //This is a normal State class
    State<Home>
    with
        //This automatically adds the ability to call setState() on the model
        AttachesSetState<Home, AppViewModel> {
  @override
  Widget build(BuildContext context) {
    final viewModel = SetStatePropagator.of<AppViewModel>(context).stateSetter;
    return Scaffold(
      appBar: AppBar(
        title:
            const Text("Managing Up The State with Management-like Managers"),
      ),
      body: Stack(children: [
        viewModel.displayWidgets
            ? Wrap(children: [
                CounterDisplay(viewModel: viewModel),
                CounterDisplay(viewModel: viewModel),
                CounterDisplay(viewModel: viewModel),
                CounterDisplay(viewModel: viewModel),
                CounterDisplay(viewModel: viewModel),
                CounterDisplay(viewModel: viewModel),
                CounterDisplay(viewModel: viewModel)
              ])
            : Text('X', style: Theme.of(context).textTheme.headline1),
        Align(
          alignment: Alignment.bottomRight,
          child: Row(children: [
            FloatingActionButton(
              onPressed: () => viewModel.callGetCount(),
              tooltip: 'Increment',
              child: const Icon(Icons.add),
            ),
            FloatingActionButton(
              onPressed: () => viewModel.hideWidgets(),
              tooltip: 'X',
              child: const Icon(Icons.close),
            )
          ]),
        ),
      ]),
    );
  }
}

class CounterDisplay extends StatelessWidget {
  const CounterDisplay({
    Key? key,
    required this.viewModel,
  }) : super(key: key);

  final AppViewModel viewModel;

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(10),
      child: Container(
        height: 200,
        width: 200,
        color: const Color(0xFFEEEEEE),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            const Text(
              'You have pushed the button this many times:',
            ),
            if (viewModel.isProcessing)
              const CircularProgressIndicator()
            else
              Text(
                '${viewModel.callCount}',
                style: Theme.of(context).textTheme.headline4,
              ),
          ],
        ),
      ),
    );
  }
}

/* Sorry but I have to cram the ioc_container package in here because
Dartpad doesn't support it 
https://pub.dev/packages/ioc_container
*/

///Defines a factory for the service and whether or not it is a singleton.
@immutable
class ServiceDefinition<T> {
  ///If true, only once instance of the service will be created and shared for
  ///for the lifespan of the app
  final bool isSingleton;

  ///The factory that creates the instance of the service and can access other
  ///services in this container
  final T Function(IocContainer container) factory;

  const ServiceDefinition(this.factory, {this.isSingleton = false});
}

///A built Ioc Container. To create a new IocContainer, use
///[IocContainerBuilder]. To get a service from the container, call [get].
///Builders create immutable containers unless you specify the
///isLazy option on toContainer(). You can build your own container by injecting
///service definitions and singletons here
@immutable
class IocContainer {
  ///This is only here for testing and you should not use this in your code
  @visibleForTesting
  final Map<Type, ServiceDefinition> serviceDefinitionsByType;

  ///This is only here for testing and you should not use this in your code
  @visibleForTesting
  final Map<Type, Object> singletons;

  const IocContainer(this.serviceDefinitionsByType, this.singletons);

  ///Get an instance of the service by type
  T get<T>() => singletons.containsKey(T)
      ? singletons[T] as T
      : serviceDefinitionsByType.containsKey(T)
          ? (serviceDefinitionsByType[T]!.isSingleton
              ? singletons.putIfAbsent(
                  T,
                  () =>
                      serviceDefinitionsByType[T]!.factory(this) as Object) as T
              : serviceDefinitionsByType[T]!.factory(this))
          : throw Exception('Service not found');
}

///A builder for creating an [IocContainer].
@immutable
class IocContainerBuilder {
  final Map<Type, ServiceDefinition> _serviceDefinitionsByType = {};

  ///Throw an error if a service is added more than once. Set this to true when
  ///you want to add mocks to set of services for a test.
  final bool allowOverrides;

  IocContainerBuilder({this.allowOverrides = false});

  ///Add a factory to the container.
  void addServiceDefinition<T>(

      ///Add a factory and whether or not this service is a singleton
      ServiceDefinition<T> serviceDefinition) {
    if (_serviceDefinitionsByType.containsKey(T)) {
      if (allowOverrides) {
        _serviceDefinitionsByType.remove(T);
      } else {
        throw Exception('Service already exists');
      }
    }

    _serviceDefinitionsByType.putIfAbsent(T, () => serviceDefinition);
  }

  ///Create an [IocContainer] from the [IocContainerBuilder].
  ///This will create an instance of each singleton service and store it
  ///in an immutable list unless you specify [isLazy] as true.
  IocContainer toContainer(
      {

      ///If this is true the services will be created when they are requested
      ///and this container will not technically be immutable.
      bool isLazy = false}) {
    if (!isLazy) {
      final singletons = <Type, Object>{};
      final tempContainer = IocContainer(_serviceDefinitionsByType, singletons);
      _serviceDefinitionsByType.forEach((type, serviceDefinition) {
        if (serviceDefinition.isSingleton) {
          singletons.putIfAbsent(
              type, () => serviceDefinition.factory(tempContainer));
        }
      });

      return IocContainer(
          Map<Type, ServiceDefinition>.unmodifiable(_serviceDefinitionsByType),
          Map<Type, Object>.unmodifiable(singletons));
    }
    return IocContainer(
        Map<Type, ServiceDefinition>.unmodifiable(_serviceDefinitionsByType),
        //Note: this case allows the singletons to be mutable
        // ignore: prefer_const_literals_to_create_immutables
        <Type, Object>{});
  }
}

extension Extensions on IocContainerBuilder {
  ///Add a singleton service to the container.
  void addSingletonService<T>(T service) => addServiceDefinition(
      ServiceDefinition<T>((i) => service, isSingleton: true));

  ///Add a singleton factory to the container. The container
  ///will only call this once throughout the lifespan of the app
  void addSingleton<T>(T Function(IocContainer container) factory) =>
      addServiceDefinition(ServiceDefinition(factory, isSingleton: true));

  ///Add a factory to the container.
  void add<T>(T Function(IocContainer container) factory) =>
      addServiceDefinition(ServiceDefinition(factory));
}
