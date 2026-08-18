using ERP.Application.Contract.UserAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;

namespace ERP.Application.Service
{
    public class ApplicationUser: IApplicationUser
    {
        private readonly IRepositoryUser _repositoryUser;
        private readonly IEncoder _encoder;
        private readonly IFileManager _fileManager;

        public ApplicationUser(IRepositoryUser repositoryUser, IEncoder encoder, IFileManager fileManager)
        {
            _repositoryUser = repositoryUser;
            _encoder = encoder;
            _fileManager = fileManager;
        }
        public void Register(RegisterUserDto command)
        {
            string directory = _fileManager.GetProfilePictureDirectory();
            command.UsersCriteria.PasswordHashed = _encoder.EncodeToMd5(command.Password);
            if(command.ImageFile != null)
            {
                command.UsersCriteria.ProfilePicture = _fileManager.GetUnicFileName(command.ImageFile.FileName);
                command.ImageFile.CopyTo(_fileManager.GetFileStream(command.UsersCriteria.ProfilePicture, directory));
            }
            var user = new UserModel(command.UsersCriteria);
            _repositoryUser.Register(user);
            _repositoryUser.SaveChange();

        }
        public UserViewModel Login(LoginUserViewModel command)
        {
            var user = _repositoryUser.GetBy(command.Username);
            if (user == null || (command.Username != user.UserName || !_encoder.CompareMd5Text(user.PasswordHashed, command.Password)))
                return null;

            return new UserViewModel
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                UsersCriteria = new UserCriteria
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    ProfilePicture = user.ProfilePicture,
                },
            };
        }

        public void Edit(EditUserDto Command)
        {
            var quary = _repositoryUser.GetBy(Command.Id);
            if (quary == null)
                throw new NullReferenceException();

            string directoty = _fileManager.GetProfilePictureDirectory();
            Command.UsersCriteria.PasswordHashed = _encoder.EncodeToMd5(Command.Password);
            if(Command.ImageFile != null)
            {
                Command.UsersCriteria.ProfilePicture = _fileManager.GetUnicFileName(Command.ImageFile.FileName);
                Command.ImageFile.CopyTo(_fileManager.GetFileStream(Command.UsersCriteria.ProfilePicture, directoty));
            }

            quary.Edit(Command.UsersCriteria);
            _repositoryUser.SaveChange();
        }

        public UserViewModel GetBy(int id)
        {
            var quary = _repositoryUser.GetBy(id);
            return new UserViewModel
            {
                Id = quary.Id,
                FullName = $"{quary.FirstName} {quary.LastName}",
                UsersCriteria = new UserCriteria
                {
                    UserName = quary.UserName,
                    FirstName = quary.FirstName,
                    LastName = quary.LastName,
                    PhoneNumber = quary.PhoneNumber,
                    ProfilePicture = quary.ProfilePicture,
                }
            };
        }

        public UserViewModel GetBy(string userName)
        {
            var quary = _repositoryUser.GetBy(userName);
            if (quary == null)
                return null;

            return new UserViewModel
            {
                Id = quary.Id,
                UsersCriteria = new UserCriteria
                {
                    FirstName = quary.FirstName,
                    UserName = quary.UserName,
                }
            };
        }

        public List<UserViewModel> GetAll()
        {
            return _repositoryUser.GetAll()
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    FullName = x.UserName,
                }).ToList();
        }

        public EditUserDto GetForEdit(int id)
        {
            var quary = _repositoryUser.GetBy(id);
            return new EditUserDto
            {
                Id = id,
                UsersCriteria = new UserCriteria
                {
                    FirstName = quary.FirstName,
                    LastName = quary.LastName,
                    UserName = quary.UserName,
                    PhoneNumber = quary.PhoneNumber,
                    ProfilePicture = quary.ProfilePicture,
                }
            };
        }

        public bool HasActiveUser()
        {
            var activeUsers = _repositoryUser.GetAll().Where(x => x.Active == true);
            return activeUsers.Any();
        }
    }
}
