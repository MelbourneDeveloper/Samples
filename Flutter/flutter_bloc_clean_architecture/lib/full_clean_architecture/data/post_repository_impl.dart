import 'package:clean_architecture/full_clean_architecture/data/mappers/post_mapper.dart';
import 'package:clean_architecture/full_clean_architecture/data/post_data_source.dart';
import 'package:clean_architecture/full_clean_architecture/domain/repositories/post_repository.dart';
import 'package:clean_architecture/post_state.dart';

/// Implementation of [PostRepository] following Clean Architecture principles.
class PostRepositoryImpl implements PostRepository {
  /// Constructs a [PostRepositoryImpl] with the given [dataSource].
  PostRepositoryImpl({required this.dataSource});

  /// The data source used to fetch posts.
  final PostDataSource dataSource;

  @override
  Future<PostList> getPosts() async =>
      PostList((await dataSource.fetchPosts()).map(toPost).toList());
}
