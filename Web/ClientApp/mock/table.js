import Mock from 'mockjs'
const list = Mock.mock({
  'array|10': [{
    'string': '@cname',
    'int': '@integer(1, 100)',
    'datetime': '@datetime',
    'select': 'yes',
    'multiselect': ['yes', 'no']
  }]
}).array
export default [
  {
    url: '/table/list',
    type: 'get',
    response: config => {
      return {
        code: 20000,
        data: list
      }
    }
  }
]
