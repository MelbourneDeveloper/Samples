import 'package:clean_architecture/extensions.dart';
import 'package:clean_architecture/post_cubit_simple.dart';
import 'package:clean_architecture/post_state.dart';
import 'package:http/http.dart' as http;

// Note this is not full Clean Architecture. This is not an attempt
// to implement full Clean Architecture, which has even more layering
// such as Use Cases and Domain Models.

// ignore: one_member_abstracts - <- The analyzer doesn't like one member abstract classes
abstract class PostDataSource {
  Future<PostList> fetchPosts();
}

class PostDataSourceImpl implements PostDataSource {
  PostDataSourceImpl({required this.client});
  final http.Client client;

  @override
  Future<PostList> fetchPosts() => client.fetchPosts();
}

// Repository Layer

// ignore: one_member_abstracts - <- The analyzer doesn't like one member abstract classes
abstract class PostRepository {
  Future<PostList> getPosts();
}

class PostRepositoryImpl implements PostRepository {
  PostRepositoryImpl({required this.dataSource});
  final PostDataSource dataSource;

  @override
  Future<PostList> getPosts() => dataSource.fetchPosts();
}

//Cubit Layer

/// This is the light version of Clean Architecture that most Flutter
/// apps implement. It has three layers, but doesn't map models
/// between layers.
class PostCubitCleanArchitectureLight extends PostCubit {
  PostCubitCleanArchitectureLight({required this.repository}) : super();
  final PostRepository repository;

  @override
  Future<void> fetchPosts() async {
    try {
      emit(const PostLoading());
      final posts = await repository.getPosts();
      emit(PostLoaded(posts: posts));
    } catch (e) {
      emit(PostError(message: e.toString()));
    }
  }
}
