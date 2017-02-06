import sys, getopt, re #, yaml
from jinja2 import Environment, FileSystemLoader
import ruamel.yaml


from collections import OrderedDict

# yaml.add_constructor(yaml.resolver.BaseResolver.DEFAULT_MAPPING_TAG,
#     lambda loader, node: OrderedDict(loader.construct_pairs(node)))


env = Environment(loader=FileSystemLoader('template'))

class_template = env.get_template('class.template')
entity_template = env.get_template('entities.template')
many_template = env.get_template('many.template')
index_template = env.get_template('index.template')
dbctx_template = env.get_template('dbcontext.template')
property_template = env.get_template('property.template')

doc = {}

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


def main(argv): 
    is_print = False

    try:
      opts, args = getopt.getopt(argv,"hpi:")
      if opts == [] and args == [] :
          raise getopt.GetoptError('')
    except getopt.GetoptError:
      print('generator.py -i <inputfile>')
      sys.exit(2)

    for opt, arg in opts:
        if opt == '-h':            
            print('generator.py -i <inputfile>')
            sys.exit()
        elif opt in ("-i"):
            inputfile = arg
        if opt in ("-p"):
            is_print = True

    fo = open(inputfile, 'r', encoding='utf-8')
    doc = ruamel.yaml.load(fo, ruamel.yaml.RoundTripLoader)

    entity_list = []
    foreign_list = []
    index_list = []
    entity_name_list = []
    property_list = []

    for entity_name in list(doc):
        entity_name_list.append(entity_name)
        entity = doc[entity_name]
        
        field = []
        if 'fields' in entity:
            field = entity['fields']

        attr = []
        if 'attr' in entity:
            attr = entity['attr']

        foreign = []
        if 'foreign' in entity:
            foreign = entity['foreign']

        parent = ''
        if 'super' in entity:
            parent = entity['super']        

        fields = []
        for field_name in entity['fields']:
            comment = ''            
            if field_name in field.ca.items:
                comment = field.ca.items[field_name][2].value
            comment = comment.replace('#','').replace(' ', '').replace('\n','')            
            fields.append( dict(type=field[field_name], name=field_name, attr='', comment=comment) )
        
        # attribute proccess
        if 'attr' in entity:
            for field_name in entity['attr']:
                item = list(field for field in fields if field['name'] == field_name)
                if len(item) > 0:
                    item[0]['attr'] = entity['attr'][field_name]

        # foreign relation proccess
        if foreign is not None:
            for fk in foreign:
                with_many = None
                if fk is not None:
                    result = re.findall('[A-z,0-9]+', foreign[fk])
                    if len(result) == 3:
                        with_many = result[2]
                    many_rel_str = many_template.render(entity_name=entity_name, has_foreign=fk, has_foreign_id=result[0], with_many=with_many)
                    foreign_list.append(many_rel_str)

        #index proccess
        if 'index' in entity and entity['index'] is not None:
            index_str = index_template.render(entity_name=entity_name, index=entity['index'])
            index_list.append(index_str)

        #property proccess
        if 'properties' in entity:
            for property_name in entity['properties']:
                property_str = property_template.render(entity_name=entity_name, property_type=entity['properties'][property_name], property_name=property_name)
                property_list.append(property_str)

        entity_comment = ''
        if type(doc.ca.items[entity_name][2]) is ruamel.yaml.CommentToken:
            entity_comment = doc.ca.items[entity_name][2].value.replace('#', '').replace(' ', '').replace('\n','')
        if parent != '' and parent is not None:
            entity_name += " : " + parent            
        entity_class_str = class_template.render(entity_name=entity_name, entity_comment=entity_comment,fields=fields)
        entity_list.append(entity_class_str)

    entity_all_str = entity_template.render(entities=entity_list)
    entity_file = open('MyEntities.cs', 'w', encoding='utf-8')
    entity_file.write(entity_all_str)
    entity_file.close()
    if is_print:
        print(entity_all_str)

    ctx_all_str = dbctx_template.render(rels=foreign_list, index=index_list, entities=entity_name_list, properties=property_list)
    ctx_file = open('MyDbContext.cs', 'w', encoding='utf-8')
    ctx_file.write(ctx_all_str)
    ctx_file.close()
    if is_print:
        print(ctx_all_str)

    if not is_print:
        print(" -------    OK    ------")
        print(" ------- 恭喜发财 ------")
    


if __name__ == "__main__":
   main(sys.argv[1:])