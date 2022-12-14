import 'package:mockito/annotations.dart';
import 'package:mockito/mockito.dart';
import 'package:mocktail_exceptions/shared.dart';
import 'package:test/test.dart';
import 'mockito_test.mocks.dart';

//flutter pub run build_runner build --delete-conflicting-outputs

@GenerateNiceMocks([MockSpec<TestClass>()])
void main() {
  test('test', () async {
    var mockTestClass = MockTestClass();
    when(mockTestClass.hello).thenAnswer((_) async => 'Hi');
    print(await mockTestClass.hello);
  });
}
