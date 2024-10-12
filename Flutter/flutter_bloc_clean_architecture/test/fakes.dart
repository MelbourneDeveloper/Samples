import 'dart:convert';

import 'package:clean_architecture/full_clean_architecture/data/models/data_post.dart';
import 'package:clean_architecture/full_clean_architecture/data/post_data_source.dart';
import 'package:clean_architecture/post_cubit_clean_architecture_light.dart'
    as ca_light;
import 'package:clean_architecture/post_state.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:http/http.dart' as http;
import 'package:http/testing.dart';

/// Shared fake client
final fakeClient = MockClient(
  (request) async => Future.delayed(
    const Duration(seconds: 1),
    () => http.Response(
      jsonEncode([
        {'id': 1, 'title': 'Test Post', 'body': 'This is a test post'},
        {
          'id': 2,
          'title': 'Another Post',
          'body': 'This is another post',
        },
      ]),
      200,
    ),
  ),
);

const fakePosts = PostList([
  (id: 1, title: 'Test Post', body: 'This is a test post'),
  (id: 2, title: 'Another Post', body: 'This is another post'),
]);

class FakePostDataSource implements ca_light.PostDataSource {
  @override
  Future<PostList> fetchPosts() async => Future.delayed(
        const Duration(seconds: 1),
        () => fakePosts,
      );
}

class FakePostRepository implements ca_light.PostRepository {
  @override
  Future<PostList> getPosts() async =>
      Future.delayed(const Duration(seconds: 1), () => fakePosts);
}

class FakeFullPostDataSource implements PostDataSource {
  @override
  Future<List<DataPost>> fetchPosts() async => Future<List<DataPost>>.delayed(
        const Duration(seconds: 1),
        () => fakeDomainPosts,
      );
}

const fakeDomainPosts = <DataPost>[
  DataPost(
    id: 1,
    title: 'Test Post',
    body: 'This is a test post',
  ),
  DataPost(
    id: 2,
    title: 'Another Post',
    body: 'This is another post',
  ),
];
