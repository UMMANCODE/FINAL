namespace Final_Business.DTOs.General;

public record CommentGetDto(
  int Id,
  string Content,
  string AppUserId,
  int HouseId,
  DateTime CreatedAt
);

public record CommentCreateDto(
  string Content,
  string AppUserId,
  int HouseId
);