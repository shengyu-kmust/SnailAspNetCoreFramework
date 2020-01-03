import Mock from 'mockjs'
const successCode = 20000
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
    url: '/api/crudSample/getList',
    type: 'get',
    response: config => {
      return {
        code: successCode,
        data: list
      }
    }
  },
  {
    url: '/api/crudSample/save',
    type: 'post',
    response: config => {
      return {
        code: successCode
      }
    }
  }
]
