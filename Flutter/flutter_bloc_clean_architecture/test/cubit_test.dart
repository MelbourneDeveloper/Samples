// ignore_for_file: lines_longer_than_80_chars

import 'package:clean_architecture/full_clean_architecture/data/post_repository_impl.dart';
import 'package:clean_architecture/full_clean_architecture/domain/use_cases/fetch_posts_use_case.dart';
import 'package:clean_architecture/full_clean_architecture/post_cubit_clean_architecture_full.dart';
import 'package:clean_architecture/post_cubit_clean_architecture_light.dart'
    as ca_light;
import 'package:clean_architecture/post_cubit_simple.dart';
import 'package:clean_architecture/post_state.dart';
import 'package:flutter_test/flutter_test.dart';

import 'fakes.dart';

void main() {
  group('PostCubit Tests', () {
    /// Test for [PostCubitSimple]
    test(
        'PostCubitSimple emits [PostLoading, PostLoaded] when fetchPosts is successful',
        () async {
      final cubit = PostCubitSimple(client: fakeClient);
      final expectedStates = [
        const PostLoading(),
        const PostLoaded(posts: fakePosts),
      ];

      final emittedStates = <PostState>[];
      final subscription = cubit.stream.listen(emittedStates.add);

      await cubit.fetchPosts();
      // Wait for all states to be emitted
      await Future<void>.delayed(const Duration(seconds: 2));

      expect(emittedStates.first, expectedStates.first);
      expect(emittedStates.last, expectedStates.last);

      await subscription.cancel();
      await cubit.close();
    });

    /// Test for [PostCubitCleanArchitectureLight]
    test(
        'PostCubitCleanArchitectureLight emits [PostLoading, PostLoaded] when fetchPosts is successful',
        () async {
      final cubit = ca_light.PostCubitCleanArchitectureLight(
        repository: FakePostRepository(),
      );
      final expectedStates = [
        const PostLoading(),
        const PostLoaded(posts: fakePosts),
      ];

      final emittedStates = <PostState>[];
      final subscription = cubit.stream.listen(emittedStates.add);

      await cubit.fetchPosts();
      await Future<void>.delayed(const Duration(seconds: 2));

      expect(emittedStates.first, expectedStates.first);
      expect(emittedStates.last, expectedStates.last);

      await subscription.cancel();
      await cubit.close();
    });

    /// Test for [PostCubitCleanArchitectureFull]
    test(
        'PostCubitCleanArchitectureFull emits [PostLoading, PostLoaded] when fetchPosts is successful',
        () async {
      final repository =
          PostRepositoryImpl(dataSource: FakeFullPostDataSource());
      final useCase = FetchPostsUseCase(repository);
      final cubit = PostCubitCleanArchitectureFull(fetchPostsUseCase: useCase);

      final expectedStates = [
        const PostLoading(),
        const PostLoaded(posts: fakePosts),
      ];

      final emittedStates = <PostState>[];
      final subscription = cubit.stream.listen(emittedStates.add);

      await cubit.fetchPosts();
      await Future<void>.delayed(const Duration(seconds: 2));

      expect(emittedStates.first, expectedStates.first);
      expect(emittedStates.last, expectedStates.last);

      await subscription.cancel();
      await cubit.close();
    });
  });
}
