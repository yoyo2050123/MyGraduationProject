using JapaneseLearnSystem.Areas.Members.Models;
using JapaneseLearnSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearnSystem.Services
{
    public interface IQuestionResultService
    {
        Task<List<QuestionResult>> GenerateResultsAsync(List<QuestionPage> submittedAnswers);
    }

    public class QuestionResultService : IQuestionResultService
    {
        private readonly dbJapaneseLearnSystemContext _context;

        public QuestionResultService(dbJapaneseLearnSystemContext context)
        {
            _context = context;
        }

        public async Task<List<QuestionResult>> GenerateResultsAsync(List<QuestionPage> submittedAnswers)
        {
            var resultList = new List<QuestionResult>();
            int index = 1;

            foreach (var answer in submittedAnswers)
            {
                var question = await _context.QuestionInstance
                                             .Include(q => q.QuestionOption)
                                             .FirstOrDefaultAsync(q => q.QuestionInstanceID == answer.QuestionInstanceID);
                if (question == null) continue;

                var correctOption = question.QuestionOption.FirstOrDefault(o => o.OptionID == question.AnswerOptionID);
                var userOption = question.QuestionOption.FirstOrDefault(o => o.OptionID == answer.SelectedOptionID);

                resultList.Add(new QuestionResult
                {
                    Index = index++,
                    QuestionInstanceID = question.QuestionInstanceID,
                    QuestionContent = question.QuestionContent,
                    QuestionType = question.QuestionTemplateID,
                    Options = question.QuestionOption.Select(o => o.OptionContent).ToList(),
                    UserAnswer = answer.SelectedOptionID ?? string.Empty,
                    CorrectAnswer = question.AnswerOptionID,
                    UserAnswerContent = userOption?.OptionContent ?? string.Empty,
                    CorrectAnswerContent = correctOption?.OptionContent ?? string.Empty,
                    
                });
            }

            return resultList;
        }
    }
}
