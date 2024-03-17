CREATE OR REPLACE VIEW vw_qst_answer AS
SELECT 
	a.AnswerId AS AnswerId,
	a.ResponseId AS ResponseId,
	a.QuestionId AS QuestionId,
	q.QuestionText AS QuestionText,
	q.Ord AS Ord,
	q.QuestionTypeId AS QuestionTypeId,
	qt.QuestionTypeName AS QuestionTypeName,
	a.OptionId AS OptionId,
	a.Text AS Text
FROM
	((qst_answer a
	JOIN qst_question q ON ((a.QuestionId = q.QuestionId)))
	JOIN qst_questiontype qt ON ((q.QuestionTypeId = qt.QuestionTypeId)))