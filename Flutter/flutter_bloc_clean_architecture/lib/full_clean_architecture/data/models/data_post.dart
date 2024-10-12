/// Data model representing a Post fetched from the API
class DataPost {
  /// Constructs a [DataPost] with the given [id], [title], and [body].
  const DataPost({
    required this.id,
    required this.title,
    required this.body,
  });

  /// Creates a [DataPost] instance from a JSON map.
  factory DataPost.fromJson(Map<String, dynamic> json) => DataPost(
        id: json['id'] as int,
        title: json['title'] as String,
        body: json['body'] as String,
      );
      
  /// The unique identifier of the post.
  final int id;
  
  /// The title of the post.
  final String title;
  
  /// The body content of the post.
  final String body;
}
