<template>
  <div>
    <el-table :data="tableData"
              style="width: 100%;margin-bottom: 20px;"
              row-key="id"
              border
              :tree-props="{children: 'children', hasChildren: 'hasChildren'}">
      <el-table-column prop="date"
                       label="日期"
                       sortable
                       width="180">
      </el-table-column>
      <el-table-column prop="name"
                       label="姓名"
                       sortable
                       width="180">
      </el-table-column>
      <el-table-column prop="address"
                       label="地址">
      </el-table-column>
    </el-table>


    <!-- element 2.8.2及以上才有树table -->

    <snail-simple-list-crud searchApi='queryListTree'
                            :fields='fields'
                            :tableBind='tableBind'
                            :handSearchTableDatas='handSearchTableDatas'>
      <template slot="oper">
        <div>
          <el-row>
            <el-button @click="addParent">添加同级</el-button>
            <el-button @click="addChild">添加子级</el-button>
            <el-button @click="edit">编辑</el-button>
            <el-button @click="remove">删除</el-button>
          </el-row>
        </div>
      </template>
    </snail-simple-list-crud>

  </div>
</template>

<script>
  export default {
    data() {
      return {
        keyValues: {
          genders: []
        },
        tableData1: [],
        tableData: [],
        fields: [
          {
            name: 'key',
            label: '键'
          },
          {
            name: 'value',
            label: '值'
          }
        ],
        tableBind: {
          treeProps: {
            children: 'childs',
            hasChildren: 'hasChildren'
          }
        }
      };
    },
    computed: {
    },
    created() {
      this.init();
      this.tableData = [{
        id: 1,
        date: '2016-05-02',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1518 弄'
      }, {
        id: 2,
        date: '2016-05-04',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1517 弄'
      }, {
        id: 3,
        date: '2016-05-01',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1519 弄',
        children: [{
          id: 31,
          date: '2016-05-01',
          name: '王小虎',
          address: '上海市普陀区金沙江路 1519 弄'
        }, {
          id: 32,
          date: '2016-05-01',
          name: '王小虎',
          address: '上海市普陀区金沙江路 1519 弄'
        }]
      }, {
        id: 4,
        date: '2016-05-03',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1516 弄'
      }];
    },
    methods: {
      addParent() {

      },
      addChild() {

      },
      edit() {

      },
      init() {
      },
      remove() {

      },
      handSearchTableDatas(res) {
        var data = res.data;
        this.convert(data);
        console.log(data);
        // this.tableData1 = data.childs;
        console.log(this.tableData1);

        return data.childs;
      },
      convert(data) {
        Object.assign(data, data.data);
        if (data && data.childs && data.childs.length > 0) {
          data.hasChildren = true;
          data.childs.forEach(item => {
            this.convert(item);
          });
        } else {
          data.hasChildren = false;
        }
      },
      selectFormatter(row, column, cellValue, index) {
        // return cellValue
        return this.$util.keyValueFormart(this.keyValues.yesnos, cellValue);
      },
      timeFormatter(row, column, cellValue, index) {
        return this.$dayjs(cellValue).format('YYYY-MM-DD');
      }
    }
  };
</script>
