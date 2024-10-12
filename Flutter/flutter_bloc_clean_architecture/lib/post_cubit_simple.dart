import 'package:bloc/bloc.dart';
import 'package:clean_architecture/extensions.dart';
import 'package:clean_architecture/post_state.dart';
import 'package:http/http.dart';

/// Abstract Cubit managing [PostState]s.
abstract class PostCubit extends Cubit<PostState> {
  /// Constructs a [PostCubit] with an initial state of [PostInitial].
  PostCubit() : super(const PostInitial());

  /// Initiates the process to fetch posts.
  Future<void> fetchPosts();
}

/// Simplified Cubit implementation for managing post states.
final class PostCubitSimple extends PostCubit {
  
  /// Constructs a [PostCubitSimple] with the given HTTP [client].
  PostCubitSimple({required this.client}) : super();
  
  /// The HTTP client used to fetch posts.
  final Client client;

  @override
  Future<void> fetchPosts() async {
    try {
      emit(const PostLoading());
      final posts = await client.fetchPosts();
      emit(PostLoaded(posts: posts));
    } catch (e) {
      emit(PostError(message: e.toString()));
    }
  }
}
