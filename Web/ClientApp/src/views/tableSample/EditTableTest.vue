<template>
  <div>
    <ex-edit-table :fields="fields" :rows="rows">

    </ex-edit-table>
  </div>
</template>

<script>

export default {
  data() {
    return {
      fields: [
        {
          name: 'string',
          label: '字符串',
          type: 'string'
        },
        {
          name: 'int',
          label: '数字',
          type: 'int'
        },
        {
          name: 'datetime',
          label: '时间',
          type: 'datetime'
        },
        {
          name: 'select',
          label: '单选',
          type: 'select',
          keyValues: [
            {
              key: true,
              value: '是'
            },
            {
              key: false,
              value: '否'
            }
          ]
        }
      ],
      rows: [
        {
          string: 'string',
          int: 1,
          datetime: '2020-10-01',
          select: true
        },
        {
          string: 'string',
          int: 1,
          datetime: '2020-10-01',
          select: true
        }
      ]
    }
  }
}
</script>

<style>

</style>
