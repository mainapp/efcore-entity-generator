from yaml import load
from jinja2 import Environment, FileSystemLoader
env = Environment(loader=FileSystemLoader('.'))

template = env.get_template('template.jinja2')

fo = open('test.yaml', 'r')
doc = load(fo)


for entity_name in list(doc):
    entity = doc[entity_name]
    
    field = []
    if 'fields' in entity:
        field = entity['fields']
    
    rel = []
    if 'rel' in entity:
        rel = entity['rel']

    parent = ''
    if 'parent' in entity:
        parent = entity['parent']        

    fields = []
    for field_name in entity['fields']:
        fields.append( dict(type=field[field_name], name=field_name) )
    
    if parent != '' and parent is not None:
        entity_name += " : " + parent
    s = template.render(entity_name=entity_name, fields=fields)
    print(s)