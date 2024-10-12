import 'dart:convert';
import 'package:clean_architecture/post.dart';
import 'package:clean_architecture/post_state.dart';
import 'package:http/http.dart';

extension PostClient on Client {
  Future<PostList> fetchPosts() async {
    final response =
        await this.get(Uri.parse('https://jsonplaceholder.typicode.com/posts'));

    if (response.statusCode == 200) {
      final jsonList = json.decode(response.body) as List<dynamic>;
      return PostList(
        jsonList
            .map(
              (jsonItem) => (jsonItem as Map<String, dynamic>).toPost(),
            )
            .toList(),
      );
    } else {
      throw Exception('Failed to load posts');
    }
  }
}

/// Extension methods for [Map] to convert JSON to domain models.
extension MapPostExtensions on Map<String, dynamic> {
  /// Converts a JSON map to a [Post] object.
  Post toPost() => (
        id: this['id'] as int,
        title: this['title'] as String,
        body: this['body'] as String,
      );
}
