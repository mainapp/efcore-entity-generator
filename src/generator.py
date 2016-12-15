from yaml import load
from jinja2 import Environment, FileSystemLoader
env = Environment(loader=FileSystemLoader('template'))

class_template = env.get_template('class.template')
entity_template = env.get_template('entities.template')
many_template = env.get_template('many.template')
dbctx_template = env.get_template('dbcontext.template')

fo = open('test.yaml', 'r')
doc = load(fo)

def find_entity(entity_name):
    item = list(entity for entity in doc if entity==entity_name)
    if len(item) > 0:
        return True, doc[item[0]]
    else:
        return False, None

def entity_hasmany(entity_name, rel_name):
    ret, entity = find_entity(entity_name)
    if ret and 'many' in entity:
        many = entity['many']
        return len(list(x for x in many if x == rel_name)) == 1

    

entity_list = []
many_list = []
entity_name_list = []

for entity_name in list(doc):
    entity_name_list.append(entity_name)
    entity = doc[entity_name]
    
    field = []
    if 'fields' in entity:
        field = entity['fields']

    attr = []
    if 'attr' in entity:
        attr = entity['attr']
    
    many = []
    if 'many' in entity:
        many = entity['many']

    one = []
    if 'one' in entity:
        one = entity['one']

    parent = ''
    if 'super' in entity:
        parent = entity['super']        

    fields = []
    for field_name in entity['fields']:
        fields.append( dict(type=field[field_name], name=field_name, attr='') )
    
    for field_name in entity['attr']:
        item = list(field for field in fields if field['name'] == field_name)
        if len(item) > 0:
            item[0]['attr'] = entity['attr'][field_name]

    for rel in one:
        fields.append( dict(type=rel, name=rel, attr='') )

        with_many = None
        ret = entity_hasmany(rel, entity_name)
        if ret :
            with_many = '{0}s'.format(entity_name)
        sss = many_template.render(entity_name=entity_name, has_one=rel, with_many=with_many)
        many_list.append(sss)

    for rel in many:
        fields.append( dict(type='List<{0}>'.format(rel), name='{0}s'.format(rel), attr=''))

    if parent != '' and parent is not None:
        entity_name += " : " + parent
    entity_class_str = class_template.render(entity_name=entity_name, fields=fields)
    entity_list.append(entity_class_str)

entity_all_str = entity_template.render(entities=entity_list)
print(entity_all_str)

ctx_all_str = dbctx_template.render(rels=many_list, entities=entity_name_list)
print(ctx_all_str)