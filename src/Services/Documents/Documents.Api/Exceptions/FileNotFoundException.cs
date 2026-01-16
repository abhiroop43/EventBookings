using BuildingBlocks.Exceptions;

namespace Documents.Api.Exceptions;

public class FileNotFoundException(string FileId) : NotFoundException("Attachment", FileId);