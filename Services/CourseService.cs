using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.Vendors;
using Nop.Data;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Shipping.Date;
using Nop.Services.Stores;

namespace Nop.Plugin.Misc.SimpleLMS.Services
{
    public partial class CourseService
    {
        protected readonly CatalogSettings _catalogSettings;
        protected readonly CommonSettings _commonSettings;
        protected readonly IAclService _aclService;
        protected readonly ICustomerService _customerService;
        protected readonly IDateRangeService _dateRangeService;
        protected readonly ILanguageService _languageService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IOrderService _orderService;

        protected readonly IRepository<Order> _orderRepository;
        protected readonly IRepository<OrderItem> _orderItemRepository;
        protected readonly IRepository<Product> _productRepository;


        protected readonly IRepository<Course> _courseRepository;
        protected readonly IRepository<Lesson> _lessonRepository;
        protected readonly IRepository<Section> _sectionRepository;
        protected readonly IRepository<SectionLesson> _sectionLessonRepository;
        protected readonly IRepository<CourseProgress> _courseProgressRepository;
        protected readonly IRepository<LessonProgress> _lessonProgressRepository;
        protected readonly IRepository<Attachment> _attachmentRepository;
        protected readonly IRepository<Video> _videoRepository;

        protected readonly IRepository<Customer> _customerRepository;
        protected readonly IRepository<Vendor> _vendorRepository;

        protected readonly IRepository<Category> _categoryRepository;
        protected readonly IRepository<ProductCategory> _productCategoryRepository;
        protected readonly IStaticCacheManager _staticCacheManager;
        protected readonly IStoreMappingService _storeMappingService;
        protected readonly IStoreService _storeService;
        protected readonly IWorkContext _workContext;
        protected readonly LocalizationSettings _localizationSettings;
        protected readonly IPictureService _pictureService;
        protected readonly IRepository<LessonAttachment> _lessonAttachmentrepository;

        public CourseService(CatalogSettings catalogSettings,
        CommonSettings commonSettings,
        IAclService aclService,
        ICustomerService customerService,
        IDateRangeService dateRangeService,
        ILanguageService languageService,
        ILocalizationService localizationService,
        IRepository<Product> productRepository,
        IRepository<Course> courseRepository,
        IRepository<Lesson> lessonRepository,
        IRepository<Section> sectionRepository,
        IRepository<SectionLesson> sectionLessonRepository,
        IRepository<CourseProgress> courseProgressRepository,
        IRepository<LessonProgress> lessonProgressRepository,
        IRepository<Attachment> attachmentRepository,
        IRepository<Video> videoRepository,
        IRepository<Customer> customerRepository,
        IRepository<Vendor> vendorRepository,
        IRepository<Category> categoryRepository,
        IRepository<ProductCategory> productCategoryRepository,
        IRepository<Store> storeRepository,
        IStaticCacheManager staticCacheManager,
        IStoreMappingService storeMappingService,
        IStoreService storeService,
        IWorkContext workContext,
        IRepository<Order> orderRepository,
        IRepository<OrderItem> orderItemRepository,
        LocalizationSettings localizationSettings,
        IPictureService pictureService,
        IRepository<LessonAttachment> lessonAttachmentrepository,
        IOrderService orderService)
        {
            _catalogSettings = catalogSettings;
            _commonSettings = commonSettings;
            _aclService = aclService;
            _customerService = customerService;
            _dateRangeService = dateRangeService;
            _languageService = languageService;
            _localizationService = localizationService;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _staticCacheManager = staticCacheManager;
            _storeMappingService = storeMappingService;
            _storeService = storeService;
            _workContext = workContext;
            _localizationSettings = localizationSettings;
            _categoryRepository = categoryRepository;
            _vendorRepository = vendorRepository;
            _productCategoryRepository = productCategoryRepository;

            _courseRepository = courseRepository;
            _lessonRepository = lessonRepository;
            _sectionRepository = sectionRepository;
            _sectionLessonRepository = sectionLessonRepository;
            _courseProgressRepository = courseProgressRepository;
            _lessonProgressRepository = lessonProgressRepository;
            _attachmentRepository = attachmentRepository;
            _videoRepository = videoRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _pictureService = pictureService;
            _lessonAttachmentrepository = lessonAttachmentrepository;
            _orderService = orderService;

        }

        public async Task<int> GetCourseProgressPercentByUserId(int courseId, int userId)
        {
            var lessons = await this.GetLessonsByCourseIdAsync(courseId);
            var completedLessons = (from lp in _lessonProgressRepository.Table
                                    join cp in _courseProgressRepository.Table on lp.CourseProgressId equals cp.Id
                                    where cp.StudentId == userId && cp.CourseId == courseId && lp.IsCompleted
                                    select lp).Distinct().Count();

            if (lessons.Count > 0)
                return (completedLessons * 100 / lessons.Count);
            else
                return 0;


        }

        public async Task<IList<LessonProgress>> GetCourseProgressByUserId(int courseId, int userId)
        {
            var lessons = await this.GetLessonsByCourseIdAsync(courseId);
            var query = (from lp in _lessonProgressRepository.Table
                         join cp in _courseProgressRepository.Table on lp.CourseProgressId equals cp.Id
                         where cp.StudentId == userId && cp.CourseId == courseId && lp.IsCompleted
                         select lp).Distinct();


            return await query.ToListAsync();


        }

        public async Task InsertOrUpdateLessonProgressByCourseLessonAndUser(int courseId, int userId, int lessonId, bool isCompleted, int currentlyAt)
        {
            var lessonProgress = await GetCourseLessonProgressByUserId(courseId, userId, lessonId);


            if (lessonProgress == null)
            {

                lessonProgress = new LessonProgress();

                CourseProgress courseProgress = await (from p in _courseProgressRepository.Table
                                                       where p.CourseId == courseId && p.StudentId == userId
                                                       select p).SingleOrDefaultAsync();
                if (courseProgress == null)
                {
                    courseProgress = new CourseProgress();
                    courseProgress.StudentId = userId;
                    courseProgress.CourseId = courseId;
                    await _courseProgressRepository.InsertAsync(courseProgress);
                }

                lessonProgress.CourseProgressId = courseProgress.Id;
                lessonProgress.LessonId = lessonId;
            }

            lessonProgress.IsCompleted = isCompleted;
            lessonProgress.CurrentlyAt = currentlyAt;

            await InsertOrUpdateLessionProgressAsync(lessonProgress);
        }


        public async Task<LessonProgress> GetCourseLessonProgressByUserId(int courseId, int userId, int lessonId)
        {
            var lessons = await this.GetLessonsByCourseIdAsync(courseId);
            var lesson = await (from lp in _lessonProgressRepository.Table
                                join cp in _courseProgressRepository.Table on lp.CourseProgressId equals cp.Id
                                where cp.StudentId == userId && cp.CourseId == courseId && lp.IsCompleted && lp.LessonId == lessonId
                                select lp).SingleOrDefaultAsync();


            return lesson;


        }




        public async Task<IList<Attachment>> GetAttachmentByLessonIdAsync(int id)
        {
            var query = from a in _attachmentRepository.Table
                        join l in _lessonRepository.Table on a.Id equals l.DocumentId
                        where a.InstructorId == id
                        select a;


            return await query.ToListAsync();
        }

        public async Task<IList<Category>> GetCourseCategoryByCourseIdAsync(int id)
        {
            var query = from c in _courseRepository.Table
                        join p in _productRepository.Table on c.ProductId equals p.Id
                        join pc in _productCategoryRepository.Table on p.Id equals pc.ProductId
                        join cat in _categoryRepository.Table on pc.CategoryId equals cat.Id
                        where c.Id == id
                        select cat;

            return await query.Distinct().ToListAsync();
        }

        public async Task<IPagedList<Course>> GetCoursesByInstructorIdAsync(int id,
            int pageIndex = 0,
            int pageSize = 10,
            string keyword = null)
        {
            //some databases don't support int.MaxValue
            if (pageSize == int.MaxValue)
                pageSize = int.MaxValue - 1;

            var coursesQuery = _courseRepository.Table;


            if (!string.IsNullOrEmpty(keyword))
            {
                coursesQuery = from c in coursesQuery
                               where c.Name.Contains(keyword)
                               select c;
            }

            coursesQuery = coursesQuery.Where(p => p.InstructorId == id);

            return await coursesQuery.OrderByDescending(p => p.Id).ToPagedListAsync(pageIndex, pageSize);
        }


        public async Task<CourseStat> GetCourseStatsByCourseIdAsync(int id)
        {
            var prod = _productRepository.Table.Where(p => p.Id == id).SingleOrDefault();

            var orders = (from o in _orderRepository.Table
                          join i in _orderItemRepository.Table on o.Id equals i.OrderId
                          join p in _productRepository.Table on i.ProductId equals p.Id
                          join c in _courseRepository.Table on p.Id equals c.ProductId
                          where c.Id == id
                          select o).Distinct();



            return new CourseStat()
            {
                EnrolledStudents = orders.Count(),
                Lessons = (await GetLessonsByCourseIdAsync(id)).Count,
                Sections = (await GetSectionsByCourseIdAsync(id)).Count
            };
        }

        private async Task<IList<Course>> GetUserCoursesAsync()
        {
            var customer = await _workContext.GetCurrentCustomerAsync();

            var courses = (from o in _orderRepository.Table
                           join i in _orderItemRepository.Table on o.Id equals i.OrderId
                           join p in _productRepository.Table on i.ProductId equals p.Id
                           join c in _courseRepository.Table on p.Id equals c.ProductId
                           where o.CustomerId == customer.Id
                           select c).Distinct();


            return courses.ToList();

        }

        public async Task<Course> GetCourseById(int id)
        {
            return await _courseRepository.Table.Where(p => p.Id == id).SingleOrDefaultAsync();
        }

        public async Task<Course> GetCourseByProductIdAsync(int productId)
        {
            return await _courseRepository.Table.Where(p => p.ProductId == productId).SingleOrDefaultAsync();
        }

        public async Task<Lesson> GetLessonByIdAsync(int id)
        {
            return await _lessonRepository.Table.Where(p => p.Id == id).SingleOrDefaultAsync();
        }

        public async Task<IList<SectionLesson>> GetLessonSectionByLessonIdAsync(int id)
        {
            return await _sectionLessonRepository.Table.Where(p => p.LessonId == id).ToListAsync();
        }

        public async Task<IList<Lesson>> GetLessonsByCourseIdAsync(int id)
        {
            var query = from l in _lessonRepository.Table
                        join sl in _sectionLessonRepository.Table on l.Id equals sl.LessonId
                        join s in _sectionRepository.Table on sl.SectionId equals s.Id
                        where s.CourseId == id
                        select l;


            return await query.Distinct().ToListAsync();
        }

        public async Task<Lesson> GetLessonByCourseIdAndLessonIdAsync(int courseId, int lessonId)
        {
            var query = from l in _lessonRepository.Table
                        join sl in _sectionLessonRepository.Table on l.Id equals sl.LessonId
                        join s in _sectionRepository.Table on sl.SectionId equals s.Id
                        where s.CourseId == courseId && l.Id == lessonId
                        select l;


            return await query.SingleOrDefaultAsync();
        }

        public async Task<IList<Lesson>> GetLessonsByInstructorIdAsync(int id)
        {
            var query = from l in _lessonRepository.Table
                        where l.InstructorId == id
                        select l;


            return await query.ToListAsync();
        }

        public async Task<IList<Lesson>> GetLessonsBySectionIdAsync(int id)
        {
            var query = from l in _lessonRepository.Table
                        join sl in _sectionLessonRepository.Table on l.Id equals sl.LessonId
                        join s in _sectionRepository.Table on sl.SectionId equals s.Id
                        where s.Id == id
                        select l;

            return await query.Distinct().ToListAsync();
        }

        public async Task<IList<Section>> GetSectionsByCourseIdAsync(int id)
        {
            var query = from s in _sectionRepository.Table
                        where s.CourseId == id
                        select s;

            return await query.ToListAsync();
        }

        public async Task<string> GetTextByLessonIdAsync(int id)
        {
            var query = from s in _lessonRepository.Table
                        where s.Id == id
                        select s.LessonContents;

            return await query.SingleOrDefaultAsync();
        }


        public async Task<Video> GetVideoByLessonIdAsync(int id)
        {
            var query = from s in _videoRepository.Table
                        join l in _lessonRepository.Table on s.Id equals l.VideoId
                        where l.Id == id
                        select s;

            return await query.SingleOrDefaultAsync();
        }


        public virtual async Task<IPagedList<Course>> SearchCoursesAsync(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            string keyword = null,
            IList<int> categoryIds = null,
            int ventoryId = 0,
             bool showHidden = false,
            bool? overridePublished = null,
            string instructorGUID = "",
            int? userId = null
            )
        {
            //some databases don't support int.MaxValue
            if (pageSize == int.MaxValue)
                pageSize = int.MaxValue - 1;


            var coursesQuery = _courseRepository.Table.Where(p => p.Deleted == false);


            if (!string.IsNullOrEmpty(keyword))
            {
                coursesQuery = from c in coursesQuery
                               where c.Name.Contains(keyword)
                               select c;
            }

            if (userId != null)
            {
                coursesQuery = (from o in _orderRepository.Table
                                join i in _orderItemRepository.Table on o.Id equals i.OrderId
                                join p in _productRepository.Table on i.ProductId equals p.Id
                                join c in coursesQuery on p.Id equals c.ProductId
                                where o.CustomerId == userId
                                select c);
            }

            if (!string.IsNullOrEmpty(instructorGUID))
            {
                coursesQuery = from x in coursesQuery
                               where x.InstructorGuid.ToString() == instructorGUID
                               select x;
            }


            return await coursesQuery.Distinct().OrderBy(p => p.Name).ToPagedListAsync(pageIndex, pageSize);
        }

        public bool IsUserCourse(int courseId, int userId)
        {
            var coursesQuery = _courseRepository.Table.Where(p => p.Deleted == false);

            coursesQuery = (from o in _orderRepository.Table
                            join i in _orderItemRepository.Table on o.Id equals i.OrderId
                            join p in _productRepository.Table on i.ProductId equals p.Id
                            join c in coursesQuery on p.Id equals c.ProductId
                            where o.CustomerId == userId && c.Id == courseId
                            select c);

            if (coursesQuery.Count() > 0)
                return true;

            return false;


        }

        public bool ValidateCourseSectionLessonCombination(int lessonId, int sectionId, int courseId)
        {


            if ((from l in _lessonRepository.Table
                 join sl in _sectionLessonRepository.Table on l.Id equals sl.LessonId
                 join s in _sectionRepository.Table on sl.SectionId equals s.Id
                 where s.CourseId == courseId && l.Id == lessonId && s.Id == sectionId
                 select l).Count() > 0)
                return true;

            return false;


        }


        public async Task InsertOrUpdateCourseAsync(Course course)
        {
            if (course.Id > 0)
                await _courseRepository.UpdateAsync(course);
            else
                await _courseRepository.InsertAsync(course);
        }

        public async Task InsertOrUpdateSectionAsync(Section section)
        {
            if (section.Id > 0)
                await _sectionRepository.UpdateAsync(section);
            else
                await _sectionRepository.InsertAsync(section);
        }

        public async Task InsertOrUpdateVideoAsync(Video video)
        {
            if (video.Id > 0)
                await _videoRepository.UpdateAsync(video);
            else
                await _videoRepository.InsertAsync(video);
        }

        public async Task InsertOrUpdateAttachmentAsync(Attachment attachment)
        {
            if (attachment.Id > 0)
                await _attachmentRepository.UpdateAsync(attachment);
            else
                await _attachmentRepository.InsertAsync(attachment);
        }

        public async Task InsertOrUpdateLessonAsync(Lesson lesson)
        {
            if (lesson.Id > 0)
                await _lessonRepository.UpdateAsync(lesson);
            else
                await _lessonRepository.InsertAsync(lesson);
        }

        public async Task InsertOrUpdateSectionLessonAsync(SectionLesson sectionLesson)
        {
            if (sectionLesson.Id > 0)
                await _sectionLessonRepository.UpdateAsync(sectionLesson);
            else
                await _sectionLessonRepository.InsertAsync(sectionLesson);
        }

        public async Task UpdateSectionLessonsAsync(IList<SectionLesson> sectionLessons)
        {
            await _sectionLessonRepository.UpdateAsync(sectionLessons);
        }

        public async Task<Section> GetSectionByIdAsync(int secId)
        {
            return await _sectionRepository.GetByIdAsync(secId);
        }

        public async Task DeleteSectionLessonsBySecIdAsync(int secId)
        {
            var section = await _sectionRepository.GetByIdAsync(secId);
            if (section != null)
            {
                var sectionLessons = _sectionLessonRepository.Table.Where(p => p.SectionId == secId).ToList();
                foreach (var secLesson in sectionLessons)
                {
                    //Check if lesson exist in some other place
                    //_sectionLessonRepository.Table.Where(p => p.SectionId == secLesson.Id && sectionsTobeDeletes.Contains(p.SectionId))

                    await DeleteSectionLessonAsync(secLesson.SectionId, secLesson.LessonId);
                }

                await _sectionRepository.DeleteAsync(section);
            }
        }

        public async Task DeleteSectionLessonAsync(int sectionId, int lessonId)
        {
            var sectionLesson = _sectionLessonRepository.Table.Where(x => x.SectionId == sectionId && x.LessonId == lessonId).SingleOrDefault();
            if (sectionLesson != null)
            {
                await _sectionLessonRepository.DeleteAsync(sectionLesson);


                var lesson = await _lessonRepository.GetByIdAsync(lessonId);

                await _lessonRepository.DeleteAsync(lesson);


                if (lesson.VideoId > 0)
                {
                    var video = _videoRepository.Table.Where(p => p.Id == lesson.VideoId).SingleOrDefault();
                    await _videoRepository.DeleteAsync(video);
                }

                if (lesson.PictureId != null && lesson.PictureId > 0)
                {
                    var picture = await _pictureService.GetPictureByIdAsync(Convert.ToInt32(lesson.PictureId));
                    await _pictureService.DeletePictureAsync(picture);
                }

                var lessonAttachments = _lessonAttachmentrepository.Table.Where(p => p.LessonId == lesson.Id).ToList();
                if (lessonAttachments != null)
                {
                    foreach (var ls in lessonAttachments)
                    {
                        var attachment = await _attachmentRepository.GetByIdAsync(ls.AttachmentId);
                        await _attachmentRepository.DeleteAsync(attachment);
                    }
                }
            }

        }


        public async Task DeleteVideoAsync(int id)
        {
            var video = await _videoRepository.GetByIdAsync(id);
            await _videoRepository.DeleteAsync(video);
        }

        public async Task DeleteSectionAsync(int secId)
        {
            await DeleteSectionLessonsBySecIdAsync(secId);
        }

        public async Task InsertOrUpdateLessonAttachmentAsync(LessonAttachment lessonAttachment)
        {
            await _lessonAttachmentrepository.InsertAsync(lessonAttachment);
        }

        public async Task<IList<SelectListItem>> GetProductsByCurrentUserAsync()
        {
            var vendor = _workContext.GetCurrentVendorAsync();


            var productsByKeywords = from p in _productRepository.Table
                                         // where p.VendorId == vendor.Id && p.Published
                                     orderby p.Name
                                     select new SelectListItem { Text = p.Name, Value = p.Id.ToString() };

            return await productsByKeywords.ToListAsync();
        }

        public async Task<IList<SectionLesson>> GetSectionLessonsBySectionIdAsync(int id)
        {
            return await _sectionLessonRepository.Table.Where(p => p.SectionId == id).ToListAsync();
        }

        public async Task DeleteCourseAsync(Course courseExisting)
        {
            await _courseRepository.DeleteAsync(courseExisting);
        }

        public async Task DeleteCourseFullByIdAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            var sections = await GetSectionsByCourseIdAsync(courseId);
            foreach (var section in sections)
            {
                await DeleteSectionAsync(section.Id);
            }
            await _courseRepository.DeleteAsync(course);


        }


        public async Task InsertOrUpdateLessionProgressAsync(LessonProgress lessonProgress)
        {
            if (lessonProgress.Id > 0)
                await _lessonProgressRepository.UpdateAsync(lessonProgress);
            else
                await _lessonProgressRepository.InsertAsync(lessonProgress);
        }


    }
}
