using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows;

namespace Theme.Core.Controls
{
    public class MaskedTextBox : TextBox
    {
        #region Members

        private IEnumerable<int> IndexesInteriorMask => Masked.Select((c, i) => c == PromptChar ? i : -1).Where(i => i >= 0).ToArray();
        private IEnumerable<int> IndexesInverseMask => Masked.Select((c, i) => c != PromptChar ? i : -1).Where(i => i >= 0 && !IndexesInteriorMask.Contains(i)).ToArray();
        private IEnumerable<int> IndexesLooselyPrompt => this.Text.Select((c, i) => c == PromptChar ? i : -1).Where(i => i >= 0).ToArray();

        private static bool IsInsertMode = false;
        private static int LastCaretIndex = 0;

        private List<char> IndexesTextual = new List<char>();

        #endregion Members

        #region Constructors

        public MaskedTextBox()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, null, new CanExecuteRoutedEventHandler(this.CanExecuteCut)));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, null, new CanExecuteRoutedEventHandler(this.CanExecuteCopy)));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, null, new CanExecuteRoutedEventHandler(this.CanExecutePaste)));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo, null, new CanExecuteRoutedEventHandler(this.CanExecuteUndo)));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Redo, null, new CanExecuteRoutedEventHandler(this.CanExecuteRedo)));
            this.CommandBindings.Add(new CommandBinding(EditingCommands.ToggleInsert, new ExecutedRoutedEventHandler(this.ToggleInsertExecutedCallback)));

            this.CommandBindings.Add(new CommandBinding(EditingCommands.Delete, null, new CanExecuteRoutedEventHandler(this.CanExecuteDelete)));
            this.CommandBindings.Add(new CommandBinding(EditingCommands.DeletePreviousWord, null, new CanExecuteRoutedEventHandler(this.CanExecuteDeletePreviousWord)));
            this.CommandBindings.Add(new CommandBinding(EditingCommands.DeleteNextWord, null, new CanExecuteRoutedEventHandler(this.CanExecuteDeleteNextWord)));

            this.CommandBindings.Add(new CommandBinding(EditingCommands.Backspace, null, new CanExecuteRoutedEventHandler(this.CanExecuteBackspace)));

            this.ContextMenu = null;
            this.AllowDrop = false;
        }

        #endregion Constructors

        #region Override

        protected override void OnInitialized(EventArgs e)
        {
            if (Masked == null)
            {
                return;
            }

            IndexesTextual.AddRange(Masked.ToArray());

            var pastedChars = this.Text.Select((c, i) => GetFromValueOfType(c) ? c : default(char)).Where(i => i != default(char)).ToList();
            var inputIndexes = IndexesInteriorMask.ToList();

            int difference = pastedChars.Count - inputIndexes.Count;
            if (difference > 0)
            {
                pastedChars.RemoveRange(inputIndexes.Count, difference);
            }

            for (int i = 0; i < pastedChars.Count; i++)
            {
                IndexesTextual[inputIndexes[i]] = pastedChars[i];
            }

            RefreshText();

            base.OnInitialized(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (!IsInitialized)
            {
                return;
            }

            if (this.Text.Length != Masked.Length)
            {
                RefreshText();
                RefreshCaretIndex();
            }

            base.OnTextChanged(e);
        }

        #endregion Override

        #region OnPreviewTextInput

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            char addchar;

            if (!char.TryParse(e.Text, out addchar) || !GetFromValueOfType(addchar))
            {
                e.Handled = true;
                return;
            }

            int caretIndex = this.CaretIndex;
            var selectionStart = this.SelectionStart;
            var selectionLength = this.SelectionLength;

            if (this.SelectionLength != 0)
            {
                for (var i = selectionStart; i < selectionStart + selectionLength; i++)
                {
                    if (IndexesInteriorMask.Contains(i))
                    {
                        IndexesTextual[i] = PromptChar;
                    }
                }

                RefreshText();
            }

            if (IsInsertMode || OnInsertMode)
            {
                var inputIndex = IndexesInteriorMask.ToArray().Where(i => i >= caretIndex).DefaultIfEmpty(-1).FirstOrDefault();

                if (inputIndex == -1)
                {
                    e.Handled = true;
                    return;
                }

                IndexesTextual[inputIndex] = addchar;
                LastCaretIndex = inputIndex + 1;
            }
            else if (!IsInsertMode && !OnInsertMode)
            {
                var stopIndex = IndexesInteriorMask.ToArray().Where(i => i >= caretIndex && this.Text[i] != PromptChar).DefaultIfEmpty(-1).FirstOrDefault();
                var inputIndex = IndexesLooselyPrompt.ToArray().Where(i => stopIndex != -1 ? i < stopIndex && i >= caretIndex : i >= caretIndex).DefaultIfEmpty(-1).FirstOrDefault();

                if (inputIndex == -1)
                {
                    e.Handled = true;
                    return;
                }

                IndexesTextual[inputIndex] = addchar;
                LastCaretIndex = inputIndex + 1;
            }

            base.OnPreviewTextInput(e);
        }

        #endregion OnPreviewTextInput

        #region OnKeyDown

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        #endregion OnKeyDown

        #region CanExecuteUndo & CanExecuteRedo

        private void CanExecuteUndo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        private void CanExecuteRedo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        #endregion CanExecuteUndo & CanExecuteRedo

        #region CanExecuteCut

        private void CanExecuteCut(object sender, CanExecuteRoutedEventArgs e)
        {
            var selectionStart = this.SelectionStart;
            var selectionLength = this.SelectionLength;

            if (selectionLength != 0)
            {
                var inputIndexes = IndexesInteriorMask.ToArray().Where(i => i >= selectionStart && i < selectionStart + selectionLength && this.Text[i] != PromptChar).ToArray();
                var inputChars = this.Text.ToArray().Where((c, i) => inputIndexes.Contains(i)).ToArray();

                if (inputChars.Length == 0)
                {
                    e.Handled = true;
                    return;
                }

                for (int i = 0; i < inputIndexes.Length; i++)
                {
                    IndexesTextual[inputIndexes[i]] = PromptChar;
                }

                Clipboard.SetText(string.Concat(inputChars));
                LastCaretIndex = selectionStart;

                RefreshText();
                RefreshCaretIndex();
            }

            e.CanExecute = true;
        }

        #endregion CanExecuteCut

        #region CanExecuteCopy

        private void CanExecuteCopy(object sender, CanExecuteRoutedEventArgs e)
        {
            var selectionStart = this.SelectionStart;
            var selectionLength = this.SelectionLength;

            if (selectionLength != 0)
            {
                var inputChars = this.Text.ToArray().Where((c, i) => i >= selectionStart && i < selectionStart + selectionLength && !IndexesInverseMask.Contains(i) && c != PromptChar).ToArray();

                if (inputChars.Length == 0)
                {
                    e.Handled = true;
                    return;
                }

                Clipboard.SetText(string.Concat(inputChars));
            }

            e.Handled = true;
        }

        #endregion CanExecuteCopy

        #region CanExecutePaste

        private void CanExecutePaste(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Clipboard.ContainsText() == false)
            {
                e.Handled = true;
                return;
            }

            var pastedChars = Clipboard.GetText().Select((c, i) => GetFromValueOfType(c) ? c : default(char)).Where(i => i != default(char)).ToList();

            if (pastedChars == null || pastedChars.Count == 0)
            {
                e.Handled = true;
                return;
            }

            var caretIndex = this.CaretIndex;
            var selectionStart = this.SelectionStart;
            var selectionLength = this.SelectionLength;

            if (selectionLength == 0)
            {
                var inputIndex = IndexesInteriorMask.ToArray().Where(i => i >= caretIndex).DefaultIfEmpty(-1).FirstOrDefault();
                var promptEmptyIndexes = IndexesLooselyPrompt.ToArray().Where(i => i >= inputIndex).ToList();
                var promptFilledIndexes = IndexesInteriorMask.ToArray().Where(i => i >= inputIndex).ToList();
                var promptChars = this.Text.Where((c, i) => promptFilledIndexes.Contains(i)).ToList();

                if (inputIndex == -1 || promptEmptyIndexes.Count == 0)
                {
                    e.Handled = true;
                    return;
                }

                int difference = pastedChars.Count - promptEmptyIndexes.Count;
                if (difference > 0)
                {
                    pastedChars.RemoveRange(promptEmptyIndexes.Count, difference);
                }

                pastedChars.Reverse();

                for (int i = 0; i < pastedChars.Count; i++)
                {
                    promptChars.RemoveAt(promptChars.IndexOf(PromptChar));
                    promptChars.Insert(0, pastedChars[i]);
                }

                for (int i = 0; i < promptChars.Count; i++)
                {
                    IndexesTextual[promptFilledIndexes[i]] = promptChars[i];
                }

                if (pastedChars.Count == promptFilledIndexes.Count)
                {
                    LastCaretIndex = promptFilledIndexes[pastedChars.Count - 1] + 1;
                }
                else
                {
                    LastCaretIndex = promptFilledIndexes[pastedChars.Count];
                }

                RefreshText();
                RefreshCaretIndex();

                e.Handled = true;
            }
            else
            {
                var inputIndexes = IndexesInteriorMask.ToArray().Where(c => c >= selectionStart && c < selectionStart + selectionLength).ToList();

                int difference = pastedChars.Count - inputIndexes.Count;
                if (difference > 0)
                {
                    pastedChars.RemoveRange(inputIndexes.Count, difference);
                }

                for (int i = 0; i < inputIndexes.Count; i++)
                {
                    if (i < pastedChars.Count)
                    {
                        IndexesTextual[inputIndexes[i]] = pastedChars[i];
                    }
                    else
                    {
                        IndexesTextual[inputIndexes[i]] = PromptChar;
                    }
                }

                LastCaretIndex = inputIndexes[pastedChars.Count - 1] + 1;

                RefreshText();
                RefreshCaretIndex();

                e.Handled = true;
            }
        }

        #endregion CanExecutePaste

        #region ToggleInsertExecuted

        private void ToggleInsertExecutedCallback(object sender, ExecutedRoutedEventArgs e)
        {
            IsInsertMode = Keyboard.IsKeyToggled(Key.Insert);
            e.Handled = true;
        }

        #endregion ToggleInsertExecuted

        #region CanExecuteBackspace

        private void CanExecuteBackspace(object sender, CanExecuteRoutedEventArgs e)
        {
            var caretIndex = this.CaretIndex;
            var selectionStart = this.SelectionStart;
            var selectionLength = this.SelectionLength;

            if (selectionLength == 0)
            {
                var inputIndex = IndexesInteriorMask.ToArray().Reverse().Where(i => i < caretIndex).DefaultIfEmpty(-1).FirstOrDefault();

                if (inputIndex == -1 || caretIndex - 1 != inputIndex)
                {
                    if (this.CaretIndex != 0)
                    {
                        LastCaretIndex = this.CaretIndex - 1;
                        RefreshCaretIndex();
                    }

                    e.Handled = true;
                    return;
                }

                IndexesTextual[inputIndex] = PromptChar;
                LastCaretIndex = inputIndex;
            }
            else
            {
                for (var i = selectionStart; i < selectionStart + selectionLength; i++)
                {
                    if (IndexesInteriorMask.Contains(i))
                    {
                        IndexesTextual[i] = PromptChar;
                    }
                }

                LastCaretIndex = selectionStart;
            }
        }

        #endregion CanExecuteBackspace

        #region CanExecuteDelete

        private void CanExecuteDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            var caretIndex = this.CaretIndex;
            var selectionStart = this.SelectionStart;
            var selectionLength = this.SelectionLength;

            if (selectionLength != 0)
            {
                for (var i = selectionStart; i < selectionStart + selectionLength; i++)
                {
                    if (IndexesInteriorMask.Contains(i))
                    {
                        IndexesTextual[i] = PromptChar;
                    }
                }

                LastCaretIndex = selectionStart;
            }
            else
            {
                var inputIndex = IndexesInteriorMask.ToArray().Where(i => i >= caretIndex).DefaultIfEmpty(-1).FirstOrDefault();
                var promptIndexes = IndexesInteriorMask.ToArray().Where(i => i >= caretIndex).ToList();
                var promptChars = this.Text.Where((c, i) => promptIndexes.Contains(i)).ToList();

                if (inputIndex == -1)
                {
                    e.Handled = true;
                    return;
                }

                if (inputIndex != caretIndex)
                {
                    LastCaretIndex = inputIndex;
                    RefreshCaretIndex();

                    e.Handled = true;
                    return;
                }

                promptChars.RemoveAt(0);
                promptChars.Add(PromptChar);

                for (int i = 0; i < promptChars.Count; i++)
                {
                    IndexesTextual[promptIndexes[i]] = promptChars[i];
                }

                LastCaretIndex = inputIndex;
            }
        }

        #endregion CanExecuteDelete

        #region DeletePreviousWord

        private void CanExecuteDeletePreviousWord(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        #endregion DeletePreviousWord

        #region DeleteNextWord

        private void CanExecuteDeleteNextWord(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        #endregion DeleteNextWord

        #region Masked

        public string Masked
        {
            get { return (string)GetValue(MaskedProperty); }
            set { SetValue(MaskedProperty, value); }
        }

        public static readonly DependencyProperty MaskedProperty =
            DependencyProperty.Register("Masked", typeof(string), typeof(MaskedTextBox), new PropertyMetadata(null));

        #endregion Masked

        #region ValueOfType

        public MaskedValueOfTypeEnum ValueOfType
        {
            get { return (MaskedValueOfTypeEnum)GetValue(ValueOfTypeProperty); }
            set { SetValue(ValueOfTypeProperty, value); }
        }

        public static readonly DependencyProperty ValueOfTypeProperty =
           DependencyProperty.Register("ValueOfType", typeof(MaskedValueOfTypeEnum), typeof(MaskedTextBox), new PropertyMetadata(MaskedValueOfTypeEnum.Undefined));

        #endregion ValueOfType

        #region PromptChar Property

        private static readonly char[] MaskChars = { '_', '#', '?', '&', '$', '0' };

        public char PromptChar
        {
            get { return (char)GetValue(PromptCharProperty); }
            set { this.SetValue(PromptCharProperty, value); }
        }

        public static readonly DependencyProperty PromptCharProperty =
            DependencyProperty.Register("PromptChar", typeof(char), typeof(MaskedTextBox),
                new UIPropertyMetadata('_', new PropertyChangedCallback(MaskedTextBox.PromptCharPropertyChangedCallback),
                    new CoerceValueCallback(MaskedTextBox.PromptCharCoerceValueCallback)));

        private static object PromptCharCoerceValueCallback(object sender, object value)
        {
            MaskedTextBox c = sender as MaskedTextBox;

            if (MaskChars.Contains((char)value) == false)
                throw new ArgumentException("The prompt character is invalid.");

            return value;
        }

        private static void PromptCharPropertyChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            MaskedTextBox c = sender as MaskedTextBox;

            if (c.IsInitialized)
            {
                return;
            }
        }

        #endregion PromptChar Property

        #region OnInsertMode

        public bool OnInsertMode
        {
            get { return (bool)GetValue(OnInsertModeProperty); }
            set { SetValue(OnInsertModeProperty, value); }
        }

        public static readonly DependencyProperty OnInsertModeProperty =
            DependencyProperty.Register("OnInsertMode", typeof(bool), typeof(MaskedTextBox), new PropertyMetadata(false, null));

        #endregion OnInsertMode

        #region Pulic

        public enum MaskedValueOfTypeEnum
        {
            Undefined,
            Numerical,
            Textual
        }

        #endregion

        #region Private

        private void RefreshText()
        {
            this.Text = string.Concat(IndexesTextual);
        }

        private void RefreshCaretIndex()
        {
            this.CaretIndex = LastCaretIndex;
        }

        private bool GetFromValueOfType(char c)
        {
            bool output = false;

            switch (ValueOfType)
            {
                case MaskedValueOfTypeEnum.Undefined:
                    {
                        if (char.IsNumber(c) || char.IsLetter(c))
                        {
                            output = true;
                        }
                        break;
                    }
                case MaskedValueOfTypeEnum.Numerical:
                    {
                        output = char.IsNumber(c);
                        break;
                    }
                case MaskedValueOfTypeEnum.Textual:
                    {
                        output = char.IsLetter(c);
                        break;
                    }
                default:
                    break;
            }

            if (Masked == null)
            {
                output = false;
            }

            return output;
        }

        #endregion Private
    }
}
