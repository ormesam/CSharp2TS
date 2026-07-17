// Auto-generated from TestConstants.cs

import TestEnum from './TestEnum';

const TestConstants = {
  AppName: 'MyApp',
  NullValue: null,
  Quoted: 'It\'s "quoted"\n',
  MaxPageSize: 100,
  LongValue: 9007199254740993,
  Pi: 3.14159,
  FloatValue: 1.5,
  Price: 19.99,
  IsEnabled: true,
  Separator: ';',
  DefaultEnum: TestEnum.Value2,
  UnregisteredEnum: 1,
  UndefinedEnumValue: 99,
} as const;

export default TestConstants;
