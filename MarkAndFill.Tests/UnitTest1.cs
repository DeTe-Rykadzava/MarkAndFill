using MarkAndFill.Base.Services;
using MarkAndFill.Model;

namespace MarkAndFill.Tests
{
    public class UnitTest1
    {
        public UnitTest1()
        {
            LastFilesService.InitAsync().GetAwaiter().GetResult();
            FileGroupService.InitAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task SetNewPathTest()
        {
            var file = new FileTemplate("test2", "D:\\projects\\SecurePasswordDocs\\Words\\1.docx", "D:\\projects\\SecurePasswordDocs\\Words", FileType.MarkDown);
            LastFilesService.SetFile(file);
            await LastFilesService.SaveAsync();
            var paths = LastFilesService.GetAllPaths();
            Assert.NotEmpty(paths);
        }

        [Fact]
        public async Task RemovePathTest()
        {
            var file = LastFilesService.GetAllPaths()[0];
            LastFilesService.RemoveFile(file);
            var paths = LastFilesService.GetAllPaths();
            Assert.Empty(paths);
            await LastFilesService.SaveAsync();
        }

        [Fact]
        public async Task SetNewGroupTest()
        {
            var group = new FileTempGroup("Сотрудники");
            FileGroupService.NewGroup(group);
            var groups = FileGroupService.GetAllGroups();
            Assert.NotEmpty(groups);
            await FileGroupService.SaveAsync();
        }

        [Fact]
        public async Task RemoveGroupTest()
        {
            var group = FileGroupService.GetAllGroups()[0];
            FileGroupService.RemoveGroup(group);
            var groups = FileGroupService.GetAllGroups();
            Assert.Empty(groups);
            await FileGroupService.SaveAsync();
        }

    }
}
