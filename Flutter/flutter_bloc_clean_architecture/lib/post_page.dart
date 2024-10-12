import 'package:clean_architecture/post_cubit_simple.dart';
import 'package:clean_architecture/post_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

/// A stateless widget representing the page displaying posts.
class PostPage extends StatelessWidget {
  /// Constructs a [PostPage] with the required [cubit].
  const PostPage({
    required this.cubit,
    super.key,
  });

  /// The cubit managing the state of posts.
  final PostCubit cubit;

  @override
  Widget build(BuildContext context) => Scaffold(
        appBar: AppBar(title: const Text('Posts')),
        body: BlocBuilder<PostCubit, PostState>(
          bloc: cubit,
          builder: (context, state) => switch (state) {
            PostLoading() ||
            PostInitial() =>
              const Center(child: CircularProgressIndicator()),
            PostLoaded(posts: final posts) => ListView.builder(
                key: const Key('postList'),
                itemCount: posts.length,
                itemBuilder: (_, index) {
                  final post = posts[index];
                  return ListTile(
                    title: Text(post.title),
                    subtitle: Text(post.body),
                  );
                },
              ),
            PostError(message: final message) => Center(child: Text(message)),
          },
        ),
      );
}
