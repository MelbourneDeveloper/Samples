import 'package:clean_architecture/full_clean_architecture/domain/use_cases/fetch_posts_use_case.dart';
import 'package:clean_architecture/post_cubit_simple.dart';
import 'package:clean_architecture/post_state.dart';

/// This is a more complete implementation of Clean Architecture
/// with repositories, data sources, use cases and mapping
///
///This is not an attempt to faithfully produce a Flutter app with
// actual Clean Architecture. This is based on the examples of Flutter
// apps I've encountered and read about on blogs.
/// Cubit implementation following full Clean Architecture for managing post 
/// states.
final class PostCubitCleanArchitectureFull extends PostCubit {
  /// Constructs a [PostCubitCleanArchitectureFull] with the given 
  /// [fetchPostsUseCase].
  PostCubitCleanArchitectureFull({required this.fetchPostsUseCase});
  
  /// The use case responsible for fetching posts.
  final FetchPostsUseCase fetchPostsUseCase;

  @override
  Future<void> fetchPosts() async {
    try {
      emit(const PostLoading());
      final posts = await fetchPostsUseCase();
      emit(PostLoaded(posts: posts));
    } catch (e) {
      emit(PostError(message: e.toString()));
    }
  }
}
