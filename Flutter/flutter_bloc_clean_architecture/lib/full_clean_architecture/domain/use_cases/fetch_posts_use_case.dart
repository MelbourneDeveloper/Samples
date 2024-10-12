import 'package:clean_architecture/full_clean_architecture/domain/repositories/post_repository.dart';
import 'package:clean_architecture/post.dart';
import 'package:clean_architecture/post_state.dart';

/// Use case responsible for fetching posts using the repository.
class FetchPostsUseCase {
  /// Constructs a [FetchPostsUseCase] with the given [repository].
  const FetchPostsUseCase(this.repository);
  
  /// The repository used to fetch posts.
  final PostRepository repository;
  
  /// Executes the use case to retrieve a list of [Post].
  Future<PostList> call() => repository.getPosts();
}
