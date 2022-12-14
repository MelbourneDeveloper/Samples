import 'package:mocktail/mocktail.dart';
import 'package:mocktail_exceptions/shared.dart';
import 'package:test/test.dart';

class MockTestClass extends Mock implements TestClass {}

void main() {
  test('test', () async {
    var mockTestClass = MockTestClass();
    when(() => mockTestClass.hello).thenAnswer((_) async => 'Hi');
    print(await mockTestClass.hello);
  });
}
