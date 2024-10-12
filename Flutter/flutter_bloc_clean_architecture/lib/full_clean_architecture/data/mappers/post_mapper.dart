import 'package:clean_architecture/full_clean_architecture/data/models/data_post.dart';
import 'package:clean_architecture/post.dart';

/// Provides mapper functions to convert between [DataPost] and [Post].

/// Converts a [DataPost] instance to a [Post] domain model.
Post toPost(DataPost dataPost) => (
      id: dataPost.id,
      title: dataPost.title,
      body: dataPost.body,
    );
