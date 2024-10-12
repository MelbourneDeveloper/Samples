import 'package:clean_architecture/post.dart';
import 'package:clean_architecture/post_state.dart';

/// Abstract repository defining the contract for fetching posts.
// ignore: one_member_abstracts - This could just be a function, but this it typically how Flutter Bloc codebases are designed.
abstract class PostRepository {
  /// Retrieves a list of [Post] from the data source.
  Future<PostList> getPosts();
}
