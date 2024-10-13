___

# Movie Db Assistant : Templates
version: 1.0.0
___

The template engine transforms movies data into html web pages. A **template** is constitued of
by a set of files (js,img,css;...) and by **pages** and **parts** of pages. A specific
language inside `html` allows to integrate fragments of content and some **values** from
the **movies data** and from the **properties** of the **template**, the **engine** and the **application settings**.

___

Index

- Template folder structure
- Application resources folder
- Template specification
- Template configuration
- Template data & properties
- Template language

___

## Template language

```html
<html>
{{{part}}}
{{variable}}
<div class="if-variable"></div>
<div class="if_no-variable"></div>
<div class="if_no-variable--classname"></div>
</html>
```

### naming conventions

variable name are:
- pascal case (cam with first letter minus)
- use . to build paths

exemple: 
