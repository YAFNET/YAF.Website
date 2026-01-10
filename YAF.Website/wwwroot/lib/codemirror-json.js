import { EditorView, basicSetup } from 'codemirror';
import { Compartment } from '@codemirror/state';
import { xml } from '@codemirror/lang-xml';
// Theme
import { oneDark } from '@codemirror/theme-one-dark';
document.addEventListener('DOMContentLoaded', () => {
    const editorImport = document.getElementById('Import');
    const merged = document.getElementById('MergedOutput');
    const editorView = document.getElementById('view');
    const themeConfig = new Compartment();
    loadEditor(editorImport);
    if (merged) {
        merged.value = htmlDecode(merged.value);
        loadEditor(editorView);
    }
    function loadEditor(editor) {
        if (editor && document.documentElement.dataset.bsTheme) {
            if (document.documentElement.dataset.bsTheme === 'dark') {
                editorFromTextArea(editor, [basicSetup, xml(), themeConfig.of(oneDark)]);
            }
            else {
                editorFromTextArea(editor, [basicSetup, xml()]);
            }
        }
    }
    function editorFromTextArea(textarea, extensions) {
        var _a;
        const view = new EditorView({ doc: textarea.value, extensions });
        (_a = textarea.parentNode) === null || _a === void 0 ? void 0 : _a.insertBefore(view.dom, textarea);
        textarea.style.display = 'none';
        if (textarea.form)
            textarea.form.addEventListener('submit', () => {
                textarea.value = view.state.doc.toString();
            });
        return view;
    }
    function htmlDecode(html) {
        const txt = document.createElement("textarea");
        txt.innerHTML = html;
        return txt.value;
    }
});
