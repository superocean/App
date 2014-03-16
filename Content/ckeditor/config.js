/*
Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.skin = "kama";
    config.toolbar = [
['Source', '-', 'Save'],
['Cut', 'Copy', 'Paste'],
['Undo', 'Redo', '-', 'Find', 'Replace', '-'],

['Bold', 'Italic', 'Underline', 'StrikeThrough','-', 'Subscript', 'Superscript'],
['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyFull'],
['Image','-', 'Table'],

['TextColor', 'BGColor']// No comma for the last row.
    ];
    config.htmlEncodeOutput = true;

};
