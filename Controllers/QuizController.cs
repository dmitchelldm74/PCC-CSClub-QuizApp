using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using NuGet.Protocol;
using QuizApp.DAL;
using QuizApp.Models;


namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        private readonly ILogger<QuizController> _logger;
        private readonly DBAppContext _context;

        public QuizController(DBAppContext context, ILogger<QuizController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var quizzes = _context.Quizzes.ToList();
            return View(model: quizzes);
        }

        public IActionResult CreateQuiz(string quizName = "Default Quiz Name")
        {
            StringValues name = new StringValues(quizName);
            Request.Form.TryGetValue("quizName", out name);
            var quiz = _context.Quizzes.Add(
                new Quiz
                {
                    Name = quizName
                }
            );
            _context.SaveChanges();            
            return View("TakeQuiz", model: quiz.Entity);
        }

        public IActionResult DeleteQuiz(int quizId)
        {
            _context.Questions.Where(q => q.QuizId == quizId).ExecuteDelete();
            _context.Quizzes.Where(q => q.QuizId == quizId).ExecuteDelete();
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult TakeQuiz(int quizId, bool checkAnswers = false)
        {
            var quiz = _context.Quizzes
                .Where(q => q.QuizId == quizId).SingleOrDefault();

            if (quiz == null)
                return NotFound();

            // Get the questions for the quiz.
            quiz.Questions = _context.Questions.Where(q => q.QuizId == quizId).ToList();


            var response = new Dictionary<int, bool>();
            if (checkAnswers)
            {
                StringValues answer;
                foreach (Question question in quiz.Questions)
                {
                    if (Request.Form.TryGetValue("question[" + question.QuestionId + "]", out answer))
                    {
                        response.Add(
                            question.QuestionId,
                            (answer == "on") && question.Answer
                        );
                    }
                }
                ViewData["questionResponses"] = response;
            }

            return View(model: quiz);
        }

        [HttpGet]
        [Route("api/v1/quiz")]
        public IActionResult ApiGetQuizzes()
        {
            return Json(_context.Quizzes.Include(q => q.Questions).ToList());
        }

        [HttpGet]
        [Route("api/v1/quiz/{quizId:int}")]
        public IActionResult ApiGetQuiz(int quizId)
        {
            var quiz = _context.Quizzes
                .Where(q => q.QuizId == quizId).SingleOrDefault();

            if (quiz == null)
                return NotFound();

            // Get the questions for the quiz.
            quiz.Questions = _context.Questions.Where(q => q.QuizId == quizId).ToList();
            return Json(quiz);
        }

        [HttpDelete]
        [Route("api/v1/quiz/{quizId:int}")]
        public IActionResult ApiDeleteQuiz(int quizId)
        {
            _context.Questions.Where(q => q.QuizId == quizId).ExecuteDelete();
            int numberDeletedQuizzes = _context.Quizzes.Where(q => q.QuizId == quizId).ExecuteDelete();
            _context.SaveChanges();

            if (numberDeletedQuizzes > 0)
            {
                return Json(
                    new
                    {
                        Message = "Successfully deleted quiz with id#" + quizId
                    }
                );
            }
            return Json(
                new
                {
                    Message = "No quiz deleted with id#" + quizId
                }
            );
        }
    }

    /*
     * Perhaps could use classes later on.
     */

    /*
    public class QuestionResponse
    {
        public bool Answered { get; set; }
        public bool Correct { get; set; }
    }

    public class QuestionResponseSummary
    {
        public int TotalQuestions;
        public int CorrectQuestions;
        public int WrongQuestions;
        public int UnansweredQuestions;
        public double PercentCorrect;
    }
    */
}
