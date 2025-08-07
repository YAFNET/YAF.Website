import { EditorView, basicSetup } from 'codemirror'
import { Compartment } from '@codemirror/state';
import { xml } from '@codemirror/lang-xml'

// Theme
import { oneDark } from '@codemirror/theme-one-dark';

document.addEventListener('DOMContentLoaded', () => {
	const editorImport = document.getElementById('Import') as HTMLTextAreaElement;
	const merged = document.getElementById('MergedOutput') as HTMLInputElement;
	const editorView = document.getElementById('view') as HTMLTextAreaElement;

	const themeConfig = new Compartment();

	loadEditor(editorImport);

	if (merged) {
		editorView.value = htmlDecode(merged.value);
		loadEditor(editorView);
	}

	function loadEditor(editor: HTMLTextAreaElement): void {
		if (editor && document.documentElement.dataset.bsTheme) {
			if (document.documentElement.dataset.bsTheme === 'dark') {
				editorFromTextArea(editor, [basicSetup, xml(), themeConfig.of(oneDark)]);
			} else {
				editorFromTextArea(editor, [basicSetup, xml()]);
			}
		}
	}

	

	function editorFromTextArea(textarea: HTMLTextAreaElement, extensions: any[]) {
		const view = new EditorView({ doc: textarea.value, extensions });
		textarea.parentNode?.insertBefore(view.dom, textarea);
		textarea.style.display = 'none';
		if (textarea.form)
			textarea.form.addEventListener('submit',
				() => {
					textarea.value = view.state.doc.toString();
				});
		return view;
	}

	function htmlDecode(html: string): string {
		const txt = document.createElement("textarea");
		txt.innerHTML = html;
		return txt.value;
	}

});