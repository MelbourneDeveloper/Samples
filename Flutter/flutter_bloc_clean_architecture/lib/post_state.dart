import 'package:clean_architecture/post.dart';
import 'package:collection/collection.dart';
import 'package:flutter/material.dart';

/// Shared Cubit State Classes
sealed class PostState {
  const PostState();
}

final class PostInitial extends PostState {
  const PostInitial();
}

final class PostLoading extends PostState {
  const PostLoading();
}

@immutable
final class PostList extends Iterable<Post> {
  const PostList(this._posts);

  final List<Post> _posts;

  @override
  Iterator<Post> get iterator => _posts.iterator;

  @override
  String toString() => 'PostList($_posts)';

  @override
  bool operator ==(Object other) =>
      other is PostList &&
      const ListEquality<Post>().equals(_posts, other._posts);

  Post operator [](int index) => _posts[index];

  @override
  int get hashCode => const ListEquality<Post>().hash(_posts);
}

@immutable
final class PostLoaded extends PostState {
  const PostLoaded({required this.posts});
  final PostList posts;

  @override
  String toString() => 'PostLoaded($posts)';

  @override
  bool operator ==(Object other) =>
      other is PostLoaded &&
      const IterableEquality<Post>().equals(posts, other.posts);

  @override
  int get hashCode => const IterableEquality<Post>().hash(posts);
}

final class PostError extends PostState {
  const PostError({required this.message});
  final String message;
}
