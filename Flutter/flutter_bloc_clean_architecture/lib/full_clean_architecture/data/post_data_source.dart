import 'dart:convert';

import 'package:clean_architecture/full_clean_architecture/data/models/data_post.dart';
import 'package:http/http.dart';

/// Abstract data source defining the method to fetch posts.
// ignore: one_member_abstracts - This could just be a function, but this it typically how Flutter Bloc codebases are designed.
abstract class PostDataSource {
  /// Fetches a list of [DataPost] from the API.
  Future<List<DataPost>> fetchPosts();
}

/// Implementation of [PostDataSource] using an HTTP client.
class PostDataSourceImpl implements PostDataSource {
  /// Constructs a [PostDataSourceImpl] with the given [client].
  PostDataSourceImpl({required this.client});
  
  /// The HTTP client used to perform network requests.
  final Client client;

  @override
  Future<List<DataPost>> fetchPosts() async {
    final response = await client
        .get(Uri.parse('https://jsonplaceholder.typicode.com/posts'));

    if (response.statusCode == 200) {
      final jsonList = json.decode(response.body) as List<dynamic>;
      return jsonList
          .map(
            (jsonItem) => DataPost.fromJson(jsonItem as Map<String, dynamic>),
          )
          .toList();
    } else {
      throw Exception('Failed to load posts');
    }
  }
}
