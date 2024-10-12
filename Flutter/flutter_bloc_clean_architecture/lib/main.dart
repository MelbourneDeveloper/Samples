import 'dart:async';

import 'package:clean_architecture/post_cubit_simple.dart';
import 'package:clean_architecture/post_page.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

void main() {
  final postCubit = PostCubitSimple(client: http.Client());

  runApp(AppRoot(postCubit: postCubit));
}

/// The root widget of the application.
class AppRoot extends StatefulWidget {
  /// Constructs an [AppRoot] with the required [postCubit].
  const AppRoot({
    required this.postCubit,
    super.key,
  });

  /// The cubit managing post states.
  final PostCubit postCubit;

  @override
  State<AppRoot> createState() => _AppRootState();
}

/// State class for [AppRoot] widget.
class _AppRootState extends State<AppRoot> {
  @override
  void initState() {
    super.initState();
    // Start loading the posts
    unawaited(widget.postCubit.fetchPosts());
  }

  @override
  Widget build(BuildContext context) => MaterialApp(
        debugShowCheckedModeBanner: false,
        title: 'Load Posts',
        home: PostPage(cubit: widget.postCubit),
      );
}
