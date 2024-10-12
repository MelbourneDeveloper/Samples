import 'package:clean_architecture/full_clean_architecture/data/post_repository_impl.dart';
import 'package:clean_architecture/full_clean_architecture/domain/use_cases/fetch_posts_use_case.dart';
import 'package:clean_architecture/full_clean_architecture/post_cubit_clean_architecture_full.dart'
    as ca_full;
import 'package:clean_architecture/main.dart';
import 'package:clean_architecture/post_cubit_clean_architecture_light.dart'
    as ca_light;
import 'package:clean_architecture/post_cubit_simple.dart';
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'fakes.dart';

void main() {
  /// Most straightforward code and test
  testWidgets(
    'PostPage displays posts (Simplified)',
    (tester) async => testPostPage(
      tester,
      PostCubitSimple(client: fakeClient),
    ),
  );

  /// Notice the extra ceremenony in the Clean Architecture test
  /// because of the layering.
  ///
  /// But also notice that the code achieves exactly the same thing
  /// as above.
  testWidgets(
    'PostPage displays posts (Clean Architecture Light)',
    (tester) async {
      final dataSource = ca_light.PostDataSourceImpl(client: fakeClient);
      final repository = ca_light.PostRepositoryImpl(dataSource: dataSource);

      return testPostPage(
        tester,
        ca_light.PostCubitCleanArchitectureLight(repository: repository),
      );
    },
  );

  // Now ask, why would you write tests like this? What is the point?
  // The only thing that these tests do is reduce test coverage because
  // they don't go down to the actual HTTP request.

  testWidgets(
    'PostPage displays posts (Clean Architecture Light with fake DataSource)',
    (tester) async {
      final fakeDataSource = FakePostDataSource();
      final repository =
          ca_light.PostRepositoryImpl(dataSource: fakeDataSource);

      await testPostPage(
        tester,
        ca_light.PostCubitCleanArchitectureLight(repository: repository),
      );
    },
  );

  testWidgets(
    'PostPage displays posts (Clean Architecture Light with fake Repository)',
    (tester) async {
      final fakeRepository = FakePostRepository();

      await testPostPage(
        tester,
        ca_light.PostCubitCleanArchitectureLight(repository: fakeRepository),
      );
    },
  );

  testWidgets(
    'PostPage displays posts (Full Clean Architecture)',
    (tester) async {
      final fakeDataSource = FakeFullPostDataSource();
      final repository = PostRepositoryImpl(
        dataSource: fakeDataSource,
      );
      final fetchPostsUseCase = FetchPostsUseCase(repository);

      await testPostPage(
        tester,
        ca_full.PostCubitCleanArchitectureFull(
          fetchPostsUseCase: fetchPostsUseCase,
        ),
      );
    },
  );
}

/// Verifies the PostPage waits for fetched data and displays it
Future<void> testPostPage(
  WidgetTester tester,
  PostCubit postCubit,
) async {
  await tester.pumpWidget(
    AppRoot(
      postCubit: postCubit,
    ),
  );

  // Should see spinner
  expect(find.byType(CircularProgressIndicator), findsOneWidget);

  // Wait for the cubit to load data
  await tester.pump(const Duration(seconds: 2));

  // Should not see spinner
  expect(find.byType(CircularProgressIndicator), findsNothing);

  // Should see posts
  expect(find.byType(ListTile), findsAtLeastNWidgets(2));
}
